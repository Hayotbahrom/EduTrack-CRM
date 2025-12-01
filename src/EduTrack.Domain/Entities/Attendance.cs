namespace EduTrack.Domain.Entities;

public class Attendance
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public string Remarks { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int GroupId { get; set; }
    public Group Group { get; set; }
}
