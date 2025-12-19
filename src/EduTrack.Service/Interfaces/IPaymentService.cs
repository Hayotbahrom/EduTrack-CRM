using EduTrack.Service.DTOs.Payments;
using EduTrack.Service.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Service.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> RemoveAsync(int id);
        Task<PaymentResultDto> AddAsync(PaymentCreationDto dto);
        Task<PaymentResultDto> UpdateAsync(int id, PaymentUpdateDto dto);
        Task<PaymentResultDto> GetByIdAsync(int id);
        Task<IEnumerable<PaymentResultDto>> GetAllAsync();
    }
}
