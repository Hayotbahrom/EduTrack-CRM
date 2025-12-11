using EduTrack.Service.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Service.Interfaces
{
    public interface IStudentService
    {
        Task<bool> RemoveAsync(int id);
        Task<StudentResultDto> AddAsync(StudentCreationDto dto);
        Task<StudentResultDto> UpdateAsync(int id, StudentCreationDto dto);
        Task<StudentResultDto> GetByIdAsync(int id);
        Task<IEnumerable<StudentResultDto>> GetAllAsync();
    }
}
