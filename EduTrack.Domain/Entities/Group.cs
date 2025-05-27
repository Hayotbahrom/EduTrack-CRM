using EduTrack.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Domain.Entities
{
    public class Group : Auditable
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<StudentGroup> StudentGroups { get; set; }
        public ICollection<Attendance> Attendances { get; set;  }
        public ICollection<Payment> Payments { get; set;  }
    }
}
