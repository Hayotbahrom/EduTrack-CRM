namespace EduTrack.Service.DTOs.StudentGroups;

public class BulkEnrollmentDto
{
    public int GroupId { get; set; }

    public List<int> StudentIds { get; set; } = [];
}
