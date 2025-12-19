using AutoMapper;
using EduTrack.Data.IRepositories;
using EduTrack.Domain.Entities;
using EduTrack.Service.DTOs.Attendances;
using EduTrack.Service.Exceptions;
using EduTrack.Service.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace EduTrack.Service.Services;

public class AttendanceService(IRepository<Attendance> repository, IMapper mapper) : IAttendanceService
{
    private readonly IRepository<Attendance> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AttendanceResultDto> AddAsync(AttendanceCreationDto dto)
    {
        var mappedAttendance = _mapper.Map<Attendance>(dto);
        var createdAttendance = await _repository.InsertAsync(mappedAttendance);
        var resultDto = _mapper.Map<AttendanceResultDto>(createdAttendance);
        return resultDto;
    }

    public async Task<IEnumerable<AttendanceResultDto>> GetAllAsync()
    {
        var attendances = await _repository.SelectAll().ToListAsync();
        return _mapper.Map<IEnumerable<AttendanceResultDto>>(attendances);
    }

    public async Task<AttendanceResultDto> GetByIdAsync(int id)
    {
        var attendance = await _repository.SelectByIdAsync(id)
            ?? throw new CustomException(404, "Attendance not found");

        return _mapper.Map<AttendanceResultDto>(attendance);
    }

    public Task<bool> RemoveAsync(int id)
    {
        var attendance = _repository.SelectByIdAsync(id)
            ?? throw new CustomException(404, "Attendance not found");

        return _repository.DeleteAsync(id);
    }

    public async Task<AttendanceResultDto> UpdateAsync(int id, AttendanceUpdateDto dto)
    {
        var attendance = _repository.SelectByIdAsync(id)
            ?? throw new CustomException(404, "Attendance not found");
        var mappedAttendance = new Attendance
        {
            Id = id,
            Date = dto.Date,
            IsPresent = dto.IsPresent,
            Remarks = dto.Remarks,
            StudentId = dto.StudentId,
            Student = dto.Student,
            GroupId = dto.GroupId,
            Group = dto.Group,
            UpdatedAt = DateTime.UtcNow,
        };
        var updatedAttendance = await _repository.UpdateAsync(mappedAttendance);
        return _mapper.Map<AttendanceResultDto>(updatedAttendance);
    }
}
