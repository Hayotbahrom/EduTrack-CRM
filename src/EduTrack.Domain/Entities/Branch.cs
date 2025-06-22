using EduTrack.Domain.Commons;

namespace EduTrack.Domain.Entities
{
    public class Branch : Auditable
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
