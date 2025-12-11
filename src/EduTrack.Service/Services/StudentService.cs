using EduTrack.Service.DTOs.Students;
using EduTrack.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.Services
{
    public class StudentService : IStudentService
    {
        public Task<StudentResultDto> AddAsync(StudentCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StudentResultDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StudentResultDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<StudentResultDto> UpdateAsync(int id, StudentUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
