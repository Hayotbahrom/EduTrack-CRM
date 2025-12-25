using EduTrack.Service.DTOs.StudentGroups;

namespace EduTrack.Service.Interfaces;

/// <summary>
/// StudentGroup (Enrollment) Service Interface
/// Student-ni Group-ga qo'shish va chiqarish uchun
/// </summary>
public interface IStudentGroupService
{
    /// <summary>
    /// Student-ni Group-ga ro'yxatga qo'shish
    /// </summary>
    /// <param name="dto">StudentGroupCreationDto (StudentId, GroupId)</param>
    /// <returns>Enrolled StudentGroup ma'lumotlari</returns>
    Task<StudentGroupResultDto> EnrollStudentAsync(StudentGroupCreationDto dto);

    /// <summary>
    /// Student-ni Group-dan chiqarish (o'chirish)
    /// </summary>
    /// <param name="studentId">O'quvchi ID</param>
    /// <param name="groupId">Guruh ID</param>
    /// <returns>true agar muvaffaqiyatli</returns>
    Task<bool> RemoveStudentAsync(int studentId, int groupId);

    /// <summary>
    /// Guruhda ro'yxatga qo'shilgan barcha o'quvchilar
    /// </summary>
    /// <param name="groupId">Guruh ID</param>
    /// <returns>Group-dagi StudentGroup-lar ro'yxati</returns>
    Task<IEnumerable<StudentGroupResultDto>> GetStudentsByGroupAsync(int groupId);

    /// <summary>
    /// O'quvchining ro'yxatga qo'shilgan barcha guruhlari
    /// </summary>
    /// <param name="studentId">O'quvchi ID</param>
    /// <returns>Student-ning StudentGroup-lar ro'yxati</returns>
    Task<IEnumerable<StudentGroupResultDto>> GetGroupsByStudentAsync(int studentId);

    /// <summary>
    /// Guruhda nechta o'quvchi bor ekanligini sanash
    /// </summary>
    /// <param name="groupId">Guruh ID</param>
    /// <returns>O'quvchilar soni</returns>
    Task<int> GetStudentCountByGroupAsync(int groupId);
}
