using EduTrack.Service.DTOs.Users;
using EduTrack.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.Services
{
    public class UserService : IUserService
    {
        public Task<UserResultDto> AddAsync(UserCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserResultDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserResultDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserResultDto> UpdateAsync(int id, UserUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
