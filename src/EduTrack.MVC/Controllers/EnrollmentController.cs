using EduTrack.Service.DTOs.StudentGroups;
using EduTrack.Service.Exceptions;
using EduTrack.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.MVC.Controllers;

public class EnrollmentController(
    IStudentGroupService studentGroupService,
    IGroupService groupService,
    IStudentService studentService) : Controller
{
    private readonly IStudentGroupService _studentGroupService = studentGroupService;
    private readonly IGroupService _groupService = groupService;
    private readonly IStudentService _studentService = studentService;

    // =====================================================
    // GET: StudentGroup/EnrollmentModal
    // Modal popup uchun - Student va Group-larni yuklash
    // =====================================================
    public async Task<IActionResult> EnrollmentModal()
    {
        try
        {
            var students = await _studentService.GetAllAsync();
            var groups = await _groupService.GetAllAsync();

            ViewBag.Students = students;
            ViewBag.Groups = groups;

            return PartialView("_EnrollmentModal");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Modal yuklashda xatolik: " + ex.Message;
            return PartialView("_EnrollmentModal");
        }
    }

    // =====================================================
    // POST: StudentGroup/Enroll
    // Student-ni Group-ga ro'yxatga qo'shish
    // =====================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(StudentGroupCreationDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Noto'g'ri ma'lumot" });
            }

            var result = await _studentGroupService.EnrollStudentAsync(dto);

            TempData["SuccessMessage"] = "O'quvchi muvaffaqiyatli ro'yxatga qo'shildi!";
            return Json(new { success = true, message = "Muvaffaqiyatli!" });
        }
        catch (CustomException ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Xatolik: " + ex.Message });
        }
    }

    // =====================================================
    // POST: StudentGroup/Remove
    // Student-ni Group-dan chiqarish
    // =====================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int studentId, int groupId)
    {
        try
        {
            var result = await _studentGroupService.RemoveStudentAsync(studentId, groupId);

            TempData["SuccessMessage"] = "O'quvchi ro'yxatdan chiqarildi!";
            return Json(new { success = true, message = "Muvaffaqiyatli o'chirildi!" });
        }
        catch (CustomException ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Xatolik: " + ex.Message });
        }
    }

    // =====================================================
    // GET: StudentGroup/GroupStudents/:groupId
    // Guruhda qaysi o'quvchilar bor ekanligini ko'rish
    // =====================================================
    public async Task<IActionResult> GroupStudents(int groupId)
    {
        try
        {
            var group = await _groupService.GetByIdAsync(groupId);
            var studentGroups = await _studentGroupService.GetStudentsByGroupAsync(groupId);

            ViewBag.Group = group;
            ViewBag.StudentGroups = studentGroups;

            return View(studentGroups);
        }
        catch (CustomException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Index", "Group");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "O'quvchilarni yuklashda xatolik: " + ex.Message;
            return RedirectToAction("Index", "Group");
        }
    }

    // =====================================================
    // GET: StudentGroup/StudentGroups/:studentId
    // O'quvchi qaysi guruhlarda bor ekanligini ko'rish
    // =====================================================
    public async Task<IActionResult> StudentGroups(int studentId)
    {
        try
        {
            var student = await _studentService.GetByIdAsync(studentId);
            var studentGroups = await _studentGroupService.GetGroupsByStudentAsync(studentId);

            ViewBag.Student = student;
            ViewBag.StudentGroups = studentGroups;

            return View(studentGroups);
        }
        catch (CustomException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Index", "Student");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Guruhlarni yuklashda xatolik: " + ex.Message;
            return RedirectToAction("Index", "Student");
        }
    }

    // =====================================================
    // GET: StudentGroup/ManageGroupEnrollment/:groupId
    // Guruhning enrollmentini manage qilish
    // =====================================================
    public async Task<IActionResult> ManageGroupEnrollment(int groupId)
    {
        try
        {
            var group = await _groupService.GetByIdAsync(groupId);
            var enrolledStudents = await _studentGroupService.GetStudentsByGroupAsync(groupId);
            var allStudents = await _studentService.GetAllAsync();

            // Enrolled students-ni olish
            var enrolledStudentIds = enrolledStudents
                .Select(sg => sg.StudentId)
                .ToList();

            // Available students (hali ro'yxatga qo'shilmagan)
            var availableStudents = allStudents
                .Where(s => !enrolledStudentIds.Contains(s.Id))
                .ToList();

            ViewBag.Group = group;
            ViewBag.EnrolledStudents = enrolledStudents;
            ViewBag.AvailableStudents = availableStudents;

            return View();
        }
        catch (CustomException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Index", "Group");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Xatolik: " + ex.Message;
            return RedirectToAction("Index", "Group");
        }
    }

    // =====================================================
    // AJAX: Get Students by Group (Modal uchun)
    // =====================================================
    [HttpGet]
    public async Task<IActionResult> GetStudentsByGroup(int groupId)
    {
        try
        {
            var studentGroups = await _studentGroupService.GetStudentsByGroupAsync(groupId);
            return Json(studentGroups);
        }
        catch (Exception ex)
        {
            return Json(new { error = ex.Message });
        }
    }

    // =====================================================
    // AJAX: Get Groups by Student (Modal uchun)
    // =====================================================
    [HttpGet]
    public async Task<IActionResult> GetGroupsByStudent(int studentId)
    {
        try
        {
            var studentGroups = await _studentGroupService.GetGroupsByStudentAsync(studentId);
            return Json(studentGroups);
        }
        catch (Exception ex)
        {
            return Json(new { error = ex.Message });
        }
    }

    // =====================================================
    // POST: StudentGroup/BulkEnroll
    // Bir nechta o'quvchini bir vaqtda guruhga qo'shish
    // =====================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BulkEnroll(int groupId, [FromBody] List<int> studentIds)
    {
        try
        {
            if (studentIds == null || studentIds.Count == 0)
            {
                return Json(new { success = false, message = "O'quvchilar tanlanmadi" });
            }

            var enrolledCount = 0;
            var failedCount = 0;

            foreach (var studentId in studentIds)
            {
                try
                {
                    var dto = new StudentGroupCreationDto
                    {
                        StudentId = studentId,
                        GroupId = groupId
                    };
                    await _studentGroupService.EnrollStudentAsync(dto);
                    enrolledCount++;
                }
                catch
                {
                    failedCount++;
                }
            }

            var message = $"Muvaffaqiyatli: {enrolledCount}, Xato: {failedCount}";
            TempData["SuccessMessage"] = message;

            return Json(new { success = true, message = message, enrolled = enrolledCount });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Xatolik: " + ex.Message });
        }
    }

    // =====================================================
    // POST: StudentGroup/BulkRemove
    // Bir nechta o'quvchini bir vaqtda guruhdan chiqarish
    // =====================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BulkRemove(int groupId, [FromBody] List<int> studentIds)
    {
        try
        {
            if (studentIds == null || studentIds.Count == 0)
            {
                return Json(new { success = false, message = "O'quvchilar tanlanmadi" });
            }

            var removedCount = 0;
            var failedCount = 0;

            foreach (var studentId in studentIds)
            {
                try
                {
                    await _studentGroupService.RemoveStudentAsync(studentId, groupId);
                    removedCount++;
                }
                catch
                {
                    failedCount++;
                }
            }

            var message = $"O'chirildi: {removedCount}, Xato: {failedCount}";
            TempData["SuccessMessage"] = message;

            return Json(new { success = true, message = message, removed = removedCount });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Xatolik: " + ex.Message });
        }
    }
}