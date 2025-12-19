using EduTrack.Service.DTOs.Branches;
using EduTrack.Service.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Service.Interfaces;

public interface IBranchService
{
    Task<bool> RemoveAsync(int id);
    Task<BranchResultDto> AddAsync(BranchCreationDto dto);
    Task<BranchResultDto> UpdateAsync(int id, BranchUpdateDto dto);
    Task<BranchResultDto> GetByIdAsync(int id);
    Task<IEnumerable<BranchResultDto>> GetAllAsync();

}
