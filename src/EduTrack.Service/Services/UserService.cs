using AutoMapper;
using EduTrack.Data.IRepositories;
using EduTrack.Domain.Entities;
using EduTrack.Domain.Enums;
using EduTrack.Service.DTOs.Users;
using EduTrack.Service.Exceptions;
using EduTrack.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.Services
{
    public class UserService(IRepository<User> repository, IMapper mapper) : IUserService
    {
        private readonly IRepository<User> _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<UserResultDto> AddAsync(UserCreationDto dto)
        {
            var user = await _repository.SelectAsync(u => u.Email == dto.Email);
            if (user != null)
            {
                throw new CustomException(404, "User already exist with Email");
            }

            var mappedUser = _mapper.Map<User>(dto);

            var result = await _repository.InsertAsync(mappedUser);
            return _mapper.Map<UserResultDto>(result);
        }

        public dynamic GetUserRoles()
        {
            var roles = Enum.GetValues(typeof(EduTrack.Domain.Enums.UserRole))
                    .Cast<EduTrack.Domain.Enums.UserRole>()
                    .Select(r => new { Value = (int)r, Text = r.ToString() })
                    .ToList();
            return roles;
        }

        public async Task<IEnumerable<UserResultDto>> GetAllAsync()
        {
            var users = await _repository.SelectAll().Where(r => r.IsDeleted == false).ToListAsync();
            return _mapper.Map<IEnumerable<UserResultDto>>(users);
        }

        public async Task<UserResultDto> GetByIdAsync(int id)
        {
            var user = await IsExistAsync(id);
            return _mapper.Map<UserResultDto>(user);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var existingUser = await IsExistAsync(id);

            var result = await _repository.DeleteAsync(id);
            return result;
        }

        public async Task<UserResultDto> UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await IsExistAsync(id);
            
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.Role = dto.Role;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _repository.UpdateAsync(user);

            return _mapper.Map<UserResultDto>(result);
        }


        // generate to check user existing method
        private async Task<User> IsExistAsync(int id)
        {
            var user = await _repository.SelectByIdAsync(id)
                ?? throw new CustomException(404, "User not found in this id.");
            return user;
        }
    }
}
