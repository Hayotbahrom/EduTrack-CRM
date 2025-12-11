using EduTrack.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Domain.Entities
{
    public class Student : Auditable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ParentPhoneNumber { get; set; }

        public ICollection<StudentGroup> StudentGroups { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
    }
}
