using AutoMapper;
using EduTrack.Data.IRepositories;
using EduTrack.Domain.Entities;
using EduTrack.Service.DTOs.Payments;
using EduTrack.Service.Exceptions;
using EduTrack.Service.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace EduTrack.Service.Services;

public class PaymentService(IRepository<Payment> repository, IMapper mapper) : IPaymentService
{
    private readonly IRepository<Payment> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PaymentResultDto> AddAsync(PaymentCreationDto dto)
    {
        var existingPayment = await IsPayForMonthAsync(dto);
        if (existingPayment is not null)
            throw new Exception("Payment for this month already exists for the student in this group.");

        var payment = new Payment
        {
            Amount = dto.Amount,
            PaymentDate = dto.PaymentDate,
            ForMonth = dto.ForMonth,
            Description = dto.Description,
            PaymentMethod = dto.PaymentMethod,
            StudentId = dto.StudentId,
            GroupId = dto.GroupId,
        };
        
        var result = await _repository.InsertAsync(payment);

        return _mapper.Map<PaymentResultDto>(result);
    }

    public async Task<IEnumerable<PaymentResultDto>> GetAllAsync()
    {
        var payments = await _repository.SelectAll().Where(r => r.IsDeleted == false).ToListAsync();

        return _mapper.Map<IEnumerable<PaymentResultDto>>(payments);
    }

    public async Task<PaymentResultDto> GetByIdAsync(int id)
    {
        var payment = await IsExistAsync(id);
        return _mapper.Map<PaymentResultDto>(payment);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var payment = await IsExistAsync(id);
        return await _repository.DeleteAsync(id);
    }

    public async Task<PaymentResultDto> UpdateAsync(int id, PaymentUpdateDto dto)
    {
        var paymentTask = await IsExistAsync(id);

        var mappedPayment = _mapper.Map(dto, paymentTask);
        mappedPayment.UpdatedAt = DateTime.UtcNow;

        var updatedPayment = await _repository.UpdateAsync(mappedPayment);
        
        return _mapper.Map<PaymentResultDto>(updatedPayment);
    }

    private Task<Payment> IsPayForMonthAsync(PaymentCreationDto dto)
    {
        var payment = _repository.SelectAsync(p => 
        p.StudentId == dto.StudentId && 
        p.ForMonth == dto.ForMonth && 
        p.GroupId == dto.GroupId);

        return payment;
    }
    private async Task<Payment> IsExistAsync(int id)
    {
        var payment = await _repository.SelectAll()
            .Where(p => p.Id == id && p.IsDeleted == false)
            .Include(p => p.Student)
            .Include(p => p.Group)
            .FirstOrDefaultAsync()
            ?? throw new CustomException(404, "Payment not found id");
        return payment;
    }
}
