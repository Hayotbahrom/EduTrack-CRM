using EduTrack.Service.DTOs.Attendances;
using EduTrack.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.Services
{
    public class AttendanceService : IAttendanceService
    {
        public Task<AttendanceResultDto> AddAsync(AttendanceCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AttendanceResultDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AttendanceResultDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AttendanceResultDto> UpdateAsync(int id, AttendanceUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
