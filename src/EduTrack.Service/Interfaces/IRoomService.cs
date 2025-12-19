using EduTrack.Service.DTOs.Rooms;
using EduTrack.Service.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Service.Interfaces
{
    public interface IRoomService
    {
        Task<bool> RemoveAsync(int id);
        Task<RoomResultDto> AddAsync(RoomCreationDto dto);
        Task<RoomResultDto> UpdateAsync(int id, RoomUpdateDto dto);
        Task<RoomResultDto> GetByIdAsync(int id);
        Task<IEnumerable<RoomResultDto>> GetAllAsync();
    }
}
