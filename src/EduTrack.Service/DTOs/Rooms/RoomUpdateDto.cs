using EduTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.DTOs.Rooms
{
    public class RoomUpdateDto
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public ICollection<Group> Groups { get; set; }
    }
}
