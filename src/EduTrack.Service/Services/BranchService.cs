using AutoMapper;
using EduTrack.Data.IRepositories;
using EduTrack.Domain.Entities;
using EduTrack.Service.DTOs.Branches;
using EduTrack.Service.Exceptions;
using EduTrack.Service.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace EduTrack.Service.Services;

public class BranchService(IRepository<Branch> repository, IMapper mapper) : IBranchService
{
    private readonly IRepository<Branch> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<BranchResultDto> AddAsync(BranchCreationDto dto)
    {
        var existingBranch = await IsExistBranchAsync(dto);
        if (existingBranch != null)
            throw new CustomException(404, "this Branch already exist");

        var mapperedBranch = _mapper.Map<Branch>(dto);
        var createdBranch = await _repository.InsertAsync(mapperedBranch);
        return _mapper.Map<BranchResultDto>(createdBranch);
    }

    public async Task<IEnumerable<BranchResultDto>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<BranchResultDto>>(await _repository
            .SelectAll()
            .Include(b => b.Rooms)
            .Where(r => r.IsDeleted == false)
            .ToListAsync());
    }

    public async Task<BranchResultDto> GetByIdAsync(int id)
    {
        var result = await IsValidAsync(id);
        return _mapper.Map<BranchResultDto>(result);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var branch = await IsValidAsync(id);
        return await _repository.DeleteAsync(id);
    }

    public async Task<BranchResultDto> UpdateAsync(int id, BranchUpdateDto dto)
    {
        var branch = await IsValidAsync(id);
        var mappedBranch = _mapper.Map(dto, branch);
        mappedBranch.UpdatedAt = DateTime.UtcNow;
        var updatedBranch = await _repository.UpdateAsync(mappedBranch);
        return _mapper.Map<BranchResultDto>(updatedBranch);
    }

    private async Task<Branch> IsValidAsync(int id)
    {
        var branch = await _repository.SelectByIdAsync(id)
            ?? throw new CustomException(404, "Branch not found");
        return branch;
    }
    private async Task<Branch> IsExistBranchAsync(BranchCreationDto dto)
    {
        var branch = await _repository.SelectAsync(b => b.Name == dto.Name && b.Address == dto.Address);
        return branch;
    }
}
