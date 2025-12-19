using EduTrack.Service.DTOs.Groups;
using EduTrack.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.Services
{
    public class GroupService : IGroupService
    {
        public Task<GroupResultDto> AddAsync(GroupCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GroupResultDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GroupResultDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GroupResultDto> UpdateAsync(int id, GroupUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
