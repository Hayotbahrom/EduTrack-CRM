using EduTrack.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Domain.Entities
{
    public class Payment : Auditable
    {
        public decimal Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string ForMoth { get; set; } // e.g., January, February, etc. or "Full Year" for annual payments
        public string Description { get; set; } // e.g., Tuition, Exam Fee, etc.
        public string PaymentMethod { get; set; } // e.g., Cash, Card, Online

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
