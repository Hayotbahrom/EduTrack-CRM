using EduTrack.Domain.Commons;

namespace EduTrack.Domain.Entities;

public class Attendance : Auditable
{
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public string? Remarks { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int GroupId { get; set; }
    public Group Group { get; set; }
}
