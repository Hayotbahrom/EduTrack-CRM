using EduTrack.Service.DTOs.Rooms;
using EduTrack.Service.DTOs.Branches;
using EduTrack.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.MVC.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IBranchService _branchService;

        public RoomController(IRoomService roomService, IBranchService branchService)
        {
            _roomService = roomService;
            _branchService = branchService;
        }

        // GET: Room/Index
        public async Task<ActionResult> Index()
        {
            try
            {
                var rooms = await _roomService.GetAllAsync();
                return View(rooms);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading rooms.";
                return View(new List<RoomResultDto>());
            }
        }

        // GET: Room/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var room = await _roomService.GetByIdAsync(id);
                if (room == null)
                {
                    TempData["ErrorMessage"] = "Room not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(room);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading room details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Room/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                var branches = await _branchService.GetAllAsync();
                ViewBag.Branches = branches;
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading branches.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Room/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoomCreationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var branches = await _branchService.GetAllAsync();
                    ViewBag.Branches = branches;
                    return View(dto);
                }

                var createdRoom = await _roomService.AddAsync(dto);
                TempData["SuccessMessage"] = $"Room '{createdRoom.Name}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var branches = await _branchService.GetAllAsync();
                ViewBag.Branches = branches;
                return View(dto);
            }
        }

        // GET: Room/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var room = await _roomService.GetByIdAsync(id);
                if (room == null)
                {
                    TempData["ErrorMessage"] = "Room not found.";
                    return RedirectToAction(nameof(Index));
                }

                var updateDto = new RoomUpdateDto
                {
                    Id = room.Id,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Description = room.Description,
                    BranchId = room.BranchId
                };

                var branches = await _branchService.GetAllAsync();
                ViewBag.Branches = branches;

                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading room for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Room/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, RoomUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var branches = await _branchService.GetAllAsync();
                    ViewBag.Branches = branches;
                    return View(dto);
                }

                if (id != dto.Id)
                {
                    ModelState.AddModelError(string.Empty, "Room ID mismatch.");
                    var branches = await _branchService.GetAllAsync();
                    ViewBag.Branches = branches;
                    return View(dto);
                }

                var updatedRoom = await _roomService.UpdateAsync(id, dto);
                TempData["SuccessMessage"] = $"Room '{updatedRoom.Name}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var branches = await _branchService.GetAllAsync();
                ViewBag.Branches = branches;
                return View(dto);
            }
        }

        // GET: Room/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var room = await _roomService.GetByIdAsync(id);
                if (room == null)
                {
                    TempData["ErrorMessage"] = "Room not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(room);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading room for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Room/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var room = await _roomService.GetByIdAsync(id);
                if (room == null)
                {
                    TempData["ErrorMessage"] = "Room not found.";
                    return RedirectToAction(nameof(Index));
                }

                await _roomService.RemoveAsync(id);
                TempData["SuccessMessage"] = $"Room '{room.Name}' deleted successfully!";
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