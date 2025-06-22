using EduTrack.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduTrack.Domain.Entities
{
    public class StudentGroup : Auditable
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
