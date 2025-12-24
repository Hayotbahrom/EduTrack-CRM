using EduTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.DTOs.Rooms
{
    public class RoomCreationDto
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }

        public int BranchId { get; set; }
    }
}
