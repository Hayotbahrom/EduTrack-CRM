using EduTrack.Service.DTOs.Rooms;
using EduTrack.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.Services
{
    public class RoomService : IRoomService
    {
        public Task<RoomResultDto> AddAsync(RoomCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RoomResultDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RoomResultDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<RoomResultDto> UpdateAsync(int id, RoomUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
