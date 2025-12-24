using EduTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.DTOs.Groups
{
    public class GroupUpdateDto
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public decimal MonthlyFee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public int TeacherId { get; set; }
        public User Teacher { get; set; }
    }
}
