using EduTrack.Domain.Entities;


namespace EduTrack.Service.DTOs.Groups
{
    public class GroupUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public decimal MonthlyFee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int RoomId { get; set; }

        public int BranchId { get; set; }

        public int TeacherId { get; set; }
    }
}
