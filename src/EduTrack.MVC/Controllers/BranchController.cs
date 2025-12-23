using EduTrack.Service.DTOs.Branches;
using EduTrack.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.MVC.Controllers
{
    public class BranchController : Controller
    {
        private readonly IBranchService _service;

        public BranchController(IBranchService service)
        {
            _service = service;
        }

        // GET: Branch/Index
        public async Task<ActionResult> Index()
        {
            try
            {
                var branches = await _service.GetAllAsync();
                return View(branches);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading branches.";
                return View(new List<BranchResultDto>());
            }
        }

        // GET: Branch/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var branch = await _service.GetByIdAsync(id);
                if (branch == null)
                {
                    TempData["ErrorMessage"] = "Branch not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(branch);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading branch details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Branch/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Branch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BranchCreationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                var createdBranch = await _service.AddAsync(dto);
                TempData["SuccessMessage"] = $"Branch '{createdBranch.Name}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: Branch/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var branch = await _service.GetByIdAsync(id);
                if (branch == null)
                {
                    TempData["ErrorMessage"] = "Branch not found.";
                    return RedirectToAction(nameof(Index));
                }

                var updateDto = new BranchUpdateDto
                {
                    Id = branch.Id,
                    Name = branch.Name,
                    Address = branch.Address,
                    PhoneNumber = branch.PhoneNumber
                };

                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading branch for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Branch/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, BranchUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                if (id != dto.Id)
                {
                    ModelState.AddModelError(string.Empty, "Branch ID mismatch.");
                    return View(dto);
                }

                var updatedBranch = await _service.UpdateAsync(id, dto);
                TempData["SuccessMessage"] = $"Branch '{updatedBranch.Name}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        // GET: Branch/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var branch = await _service.GetByIdAsync(id);
                if (branch == null)
                {
                    TempData["ErrorMessage"] = "Branch not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(branch);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading branch for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Branch/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var branch = await _service.GetByIdAsync(id);
                if (branch == null)
                {
                    TempData["ErrorMessage"] = "Branch not found.";
                    return RedirectToAction(nameof(Index));
                }

                await _service.RemoveAsync(id);
                TempData["SuccessMessage"] = $"Branch '{branch.Name}' deleted successfully!";
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