using EduTrack.Service.DTOs.Users;
using EduTrack.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

namespace EduTrack.MVC.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly IEnumHelperService _enumHelper;

        public UserController(IUserService service, IEnumHelperService enumHelper)
        {
            _service = service;
            _enumHelper = enumHelper;
        }

        // GET: User/Index
        public async Task<ActionResult> Index()
        {
            try
            {
                var users = await _service.GetAllAsync();
                return View(users);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading users.";
                return View(new List<UserResultDto>());
            }
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var user = await _service.GetByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading user details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.Roles = _enumHelper.GetRoleList();
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserCreationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Roles = _enumHelper.GetRoleList();
                    return View(dto);
                }

                // Hash password
                if (string.IsNullOrEmpty(dto.PasswordHash))
                {
                    ModelState.AddModelError(string.Empty, "Password is required.");
                    ViewBag.Roles = _enumHelper.GetRoleList();
                    return View(dto);
                }

                dto.PasswordHash = HashPassword(dto.PasswordHash);

                var createdUser = await _service.AddAsync(dto);
                TempData["SuccessMessage"] = $"User '{createdUser.FirstName} {createdUser.LastName}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Roles = _enumHelper.GetRoleList();
                return View(dto);
            }
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var user = await _service.GetByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                var updateDto = new UserUpdateDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Role = user.Role,
                };

                ViewBag.Roles = _enumHelper.GetRoleList();

                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading user for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UserUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Roles = _enumHelper.GetRoleList();
                    return View(dto);
                }

                if (id != dto.Id)
                {
                    ModelState.AddModelError(string.Empty, "User ID mismatch.");
                    ViewBag.Roles = _enumHelper.GetRoleList();
                    return View(dto);
                }

                var updatedUser = await _service.UpdateAsync(id, dto);
                TempData["SuccessMessage"] = $"User '{updatedUser.FirstName} {updatedUser.LastName}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,  ex.Message);
                ViewBag.Roles = _enumHelper.GetRoleList();
                return View(dto);
            }
        }

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var user = await _service.GetByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading user for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var user = await _service.GetByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                await _service.RemoveAsync(id);
                TempData["SuccessMessage"] = $"User '{user.FirstName} {user.LastName}' deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // Helper method to hash password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}