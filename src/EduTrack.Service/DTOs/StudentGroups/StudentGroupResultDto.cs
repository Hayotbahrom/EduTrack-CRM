using EduTrack.Domain.Entities;

namespace EduTrack.Service.DTOs.StudentGroups;

public class StudentGroupResultDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int GroupId { get; set; }
    public Group Group { get; set; }
}
