using EduTrack.Service.DTOs.Branches;
using EduTrack.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.Services
{
    public class BranchService : IBranchService
    {
        public Task<BranchResultDto> AddAsync(BranchCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BranchResultDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BranchResultDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BranchResultDto> UpdateAsync(int id, BranchUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
