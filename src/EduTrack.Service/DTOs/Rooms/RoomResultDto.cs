using EduTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.DTOs.Rooms
{
    public class RoomResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Group>? Groups { get; set; }
    }
}
