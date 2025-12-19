using EduTrack.Service.DTOs.Attendances;
using EduTrack.Service.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Service.Interfaces;

public interface IAttendanceService
{
    Task<bool> RemoveAsync(int id);
    Task<AttendanceResultDto> AddAsync(AttendanceCreationDto dto);
    Task<AttendanceResultDto> UpdateAsync(int id, AttendanceUpdateDto dto);
    Task<AttendanceResultDto> GetByIdAsync(int id);
    Task<IEnumerable<AttendanceResultDto>> GetAllAsync();
}
