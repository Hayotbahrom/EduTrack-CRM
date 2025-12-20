using EduTrack.Service.DTOs.Students;
using EduTrack.Service.Exceptions;
using EduTrack.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.MVC.Controllers
{
    public class StudentController(IStudentService service) : Controller
    {
        private readonly IStudentService _service = service;

        // GET: Student/Index
        public async Task<ActionResult> Index()
        {
            try
            {
                var students = await _service.GetAllAsync();
                return View(students);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading students.";
                return View(new List<StudentResultDto>());
            }
        }

        // GET: Student/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var student = await _service.GetByIdAsync(id);
                if (student == null)
                {
                    TempData["ErrorMessage"] = "Student not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(student);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading student details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StudentCreationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                var createdStudent = await _service.AddAsync(dto);
                TempData["SuccessMessage"] = $"Student {createdStudent.FirstName} {createdStudent.LastName} created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: Student/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var student = await _service.GetByIdAsync(id);
                if (student == null)
                {
                    TempData["ErrorMessage"] = "Student not found.";
                    return RedirectToAction(nameof(Index));
                }

                var updateDto = new StudentUpdateDto
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    BirthDate = student.BirthDate,
                    Address = student.Address,
                    PhoneNumber = student.PhoneNumber,
                    ParentPhoneNumber = student.ParentPhoneNumber
                };

                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading student data for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, StudentUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                var updatedStudent = await _service.UpdateAsync(id, dto);
                TempData["SuccessMessage"] = $"Student {updatedStudent.FirstName} {updatedStudent.LastName} updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: Student/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var student = await _service.GetByIdAsync(id);
                if (student == null)
                {
                    TempData["ErrorMessage"] = "Student not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(student);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading student for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (CustomException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}