using AutoMapper;
using EduTrack.Data.IRepositories;
using EduTrack.Domain.Entities;
using EduTrack.Service.DTOs.Rooms;
using EduTrack.Service.Exceptions;
using EduTrack.Service.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace EduTrack.Service.Services;

public class RoomService(IRepository<Room> repository, IMapper mapper) : IRoomService
{
    private readonly IRepository<Room> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<RoomResultDto> AddAsync(RoomCreationDto dto)
    {
        Room mappedRoom = _mapper.Map<Room>(dto);

        var createdRoom = await _repository.InsertAsync(mappedRoom);

        return _mapper.Map<RoomResultDto>(createdRoom);
    }

    public async Task<IEnumerable<RoomResultDto>> GetAllAsync()
    {
        var rooms = await _repository.SelectAll().ToListAsync();
        return _mapper.Map<IEnumerable<RoomResultDto>>(rooms);
    }

    public async Task<RoomResultDto> GetByIdAsync(int id)
    {
        var room = await IsExistAsync(id);
        return _mapper.Map<RoomResultDto>(room);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var result = await IsExistAsync(id);
        return await _repository.DeleteAsync(result.Id);
    }

    public async Task<RoomResultDto> UpdateAsync(int id, RoomUpdateDto dto)
    {
        var room = await IsExistAsync(id);

        var mappedRoom = _mapper.Map(dto, room);

        var updatedRoom = await _repository.UpdateAsync(mappedRoom);

        return _mapper.Map<RoomResultDto>(updatedRoom);
    }

    private async Task<Room> IsExistAsync(int id)
    {
        var room = await _repository.SelectByIdAsync(id)
            ?? throw new CustomException(404, "Room not found");

        return room;
    }
}
