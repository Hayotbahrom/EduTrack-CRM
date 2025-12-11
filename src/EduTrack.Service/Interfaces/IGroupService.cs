using EduTrack.Service.DTOs.Groups;
using EduTrack.Service.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Service.Interfaces
{
    public interface IGroupService
    {
        Task<bool> RemoveAsync(int id);
        Task<GroupResultDto> AddAsync(GroupCreationDto dto);
        Task<GroupResultDto> UpdateAsync(int id, GroupUpdateDto dto);
        Task<GroupResultDto> GetByIdAsync(int id);
        Task<IEnumerable<GroupResultDto>> GetAllAsync();
    }
}
