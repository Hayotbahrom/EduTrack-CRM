using EduTrack.Service.DTOs.Groups;
using EduTrack.Service.DTOs.Rooms;
using EduTrack.Service.Exceptions;
using EduTrack.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.MVC.Controllers;

public class GroupController(
    IGroupService groupService,
    IBranchService branchService,
    IRoomService roomService,
    IUserService userService) : Controller
{
    private readonly IGroupService _service = groupService;
    private readonly IBranchService _branchService = branchService;
    private readonly IRoomService _roomService = roomService;
    private readonly IUserService _userService = userService;

    // GET: Group/Index
    public async Task<ActionResult> Index()
    {
        try
        {
            var groups = await _service.GetAllAsync();
            return View(groups);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while loading groups.";
            return View(new List<GroupResultDto>());
        }
    }

    public async Task<ActionResult> Details(int id)
    {
        try
        {
            var group = await _service.GetByIdAsync(id);
            return View(group);
        }
        catch (CustomException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while loading student details.";
            return RedirectToAction(nameof(Index));
        }   
    }

    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var group = await _service.GetByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (CustomException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while deleting the group." + "\n" + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Group/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var group = await _service.RemoveAsync(id);

            await _service.RemoveAsync(id);
            TempData["SuccessMessage"] = $"Group deleted successfully!";
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

    public async Task<ActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(GroupCreationDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var branches = await _branchService.GetAllAsync();
                var teachers = await _userService.GetAllTeachersAsync();
                var rooms = await _roomService.GetAllByBranchIdAsync(dto.BranchId);

                ViewBag.Branches = branches;
                ViewBag.Teachers = teachers;
                ViewBag.Rooms = rooms;
                return View(dto);
            }
            var createdGroup = await _service.AddAsync(dto);
            TempData["SuccessMessage"] = $"Group created successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (CustomException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            await PopulateDropdownsAsync();
            return View(dto);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while creating the group." + "\n" + ex.Message;
            await PopulateDropdownsAsync();
            return View(dto);
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        try
        {
            var group = await _service.GetByIdAsync(id);
            var branches = await _branchService.GetAllAsync();
            var rooms = await _roomService.GetAllByBranchIdAsync(group.BranchId);
            var teachers = await _userService.GetAllTeachersAsync();

            ViewBag.Branches = branches;
            ViewBag.Rooms = rooms;
            ViewBag.Teachers = teachers;
            ViewBag.SelectedBranchId = group.BranchId;
            ViewBag.SelectedRoomId = group.RoomId;
            ViewBag.SelectedTeacherId = group.TeacherId;
            
            var updateGroup = new GroupUpdateDto
            {
                Id = group.Id,
                Name = group.Name,
                Subject = group.Subject,
                Description = group.Description,
                MonthlyFee = group.MonthlyFee,
                StartDate = group.StartDate,
                EndDate = group.EndDate,
                RoomId = group.RoomId,
                BranchId = group.BranchId,
                TeacherId = group.TeacherId,
            };

            return View(updateGroup);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while loading group for editing.\n"+ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, GroupUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var branches = await _branchService.GetAllAsync();
                var rooms = await _roomService.GetAllByBranchIdAsync(dto.BranchId);
                var teachers = await _userService.GetAllTeachersAsync();

                ViewBag.Branches = branches;
                ViewBag.Rooms = rooms;
                ViewBag.Teachers = teachers;
                return View(dto);
            }
            var updatedGroup = await _service.UpdateAsync(id, dto);
            TempData["SuccessMessage"] = $"Group updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (CustomException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            var branches = await _branchService.GetAllAsync();
            var rooms = await _roomService.GetAllByBranchIdAsync(dto.BranchId);
            var teachers = await _userService.GetAllTeachersAsync();
            ViewBag.Branches = branches;
            ViewBag.Rooms = rooms;
            ViewBag.Teachers = teachers;
            return View(dto);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while updating the group." + "\n" + ex.Message;
            return View(dto);
        }
    }

    // API: Get Rooms by Branch (AJAX uchun)
    [HttpGet]
    public async Task<IActionResult> GetRoomsByBranch(int branchId)
    {
        try
        {
            var rooms = await _roomService.GetAllByBranchIdAsync(branchId);
            return Json(rooms);
        }
        catch (Exception ex)
        {
            return Json(new { error = ex.Message });
        }
    }

    private async Task PopulateDropdownsAsync()
    {
        ViewBag.Branches = await _branchService.GetAllAsync();
        ViewBag.Teachers = await _userService.GetAllTeachersAsync();
        ViewBag.Rooms = new List<RoomResultDto>();
    }
}
