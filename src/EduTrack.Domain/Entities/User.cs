using EduTrack.Domain.Commons;
using EduTrack.Domain.Enums;

namespace EduTrack.Domain.Entities
{
    public class User : Auditable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }

        public ICollection<Group> Groups { get; set; }
    }
}
