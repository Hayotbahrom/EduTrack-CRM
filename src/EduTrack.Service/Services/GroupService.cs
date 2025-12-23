using AutoMapper;
using EduTrack.Data.IRepositories;
using EduTrack.Domain.Entities;
using EduTrack.Service.DTOs.Groups;
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
    public class GroupService(IRepository<Group> repository, IMapper mapper) : IGroupService
    {
        private readonly IRepository<Group> _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<GroupResultDto> AddAsync(GroupCreationDto dto)
        {
            var group = _mapper.Map<Group>(dto);
            
            var createdGroup = await _repository.InsertAsync(group);
            
            var resultDto = _mapper.Map<GroupResultDto>(createdGroup);
            
            return resultDto;
        }

        public async Task<IEnumerable<GroupResultDto>> GetAllAsync()
        {
            var groups = await _repository
                .SelectAll()
                .ToListAsync();
            return _mapper.Map<IEnumerable<GroupResultDto>>(groups);
        }

        public async Task<GroupResultDto> GetByIdAsync(int id)
        {
            var group = await IsExistAsync(id);
            return _mapper.Map<GroupResultDto>(group);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var group = await IsExistAsync(id);
            return await _repository.DeleteAsync(id);
        }

        public async Task<GroupResultDto> UpdateAsync(int id, GroupUpdateDto dto)
        {
            var group = await IsExistAsync(id);

            var updatedGroup = _mapper.Map(dto, group);
            updatedGroup.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(updatedGroup);
            
            return _mapper.Map<GroupResultDto>(updatedGroup);
        }

        private async Task<Group> IsExistAsync(int id)
        {
            var group = await _repository.SelectByIdAsync(id)
                ?? throw new CustomException(404, "Group not found");

            return group;
        }
    }
}
