using EduTrack.Service.DTOs.Students;
using EduTrack.Service.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Service.Interfaces
{
    public interface IUserService
    {
        Task<bool> RemoveAsync(int id);
        Task<UserResultDto> AddAsync(UserCreationDto dto);
        Task<UserResultDto> UpdateAsync(int id, UserUpdateDto dto);
        Task<UserResultDto> GetByIdAsync(int id);
        Task<IEnumerable<UserResultDto>> GetAllAsync();
    }
}
