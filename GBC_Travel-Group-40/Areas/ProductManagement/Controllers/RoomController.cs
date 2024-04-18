using GBC_Travel_Group_40.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GBC_Travel_Group_40.Areas.ProductManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using GBC_Travel_Group_40.Services;
using Microsoft.AspNetCore.Authorization;

namespace GBC_Travel_Group_40.Areas.ProductManagement.Controllers
{
    [Area("ProductManagement")]
    [Route("[area]/[controller]/[action]")]
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<FlightsController> _logger;
        private readonly ISessionService _sessionService;
        public RoomController(ApplicationDbContext db, ILogger<FlightsController> logger, ISessionService sessionService)
        {
            _db = db;
            _logger = logger;
            _sessionService = sessionService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Calling room index action");
            try
            {
                var rooms = await _db.Rooms.ToListAsync();
                _logger.LogInformation(" ---- Hello ----");
                ViewBag.flightList = rooms;
                return View(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(null);
            }
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Calling room detail action");
            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rooms room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _db.Rooms.AddAsync(room);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(room);
            }
            catch
            {
                string type = "update";
                return RedirectToAction("FailUpdataDb", "Home", new { area = "", type });
            }
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _db.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId, HotelName, Location, Checking, Checkout, PricePerNight, NumOfGuests, MaxGuests, NumberOfBeds, Rating, PetFriendly, AirConditioning, Wifi, Url")] Rooms room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(room);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await RoomsExists(room.RoomId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("DeleteConfirmed/{id:int})"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int RoomId)
        {
            try
            {
                var room = await _db.Rooms.FindAsync(RoomId);
                if (room != null)
                {
                    _db.Rooms.Remove(room);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            catch
            {
                string type = "update";
                return RedirectToAction("FailUpdataDb", "Home", new { area = "", type });
            }
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string location, string checking, string checkout, int numberOfBeds)
        {
            var roomList = _db.Rooms.AsQueryable();

            bool searchPerformed = !string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(checking) && !string.IsNullOrEmpty(checkout) && (numberOfBeds == 1 || numberOfBeds == 2);

            if (searchPerformed)
            {
                DateTime parsedCheckingDate, parsedCheckoutDate;
                if (DateTime.TryParseExact(checking, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedCheckingDate) &&
                    DateTime.TryParseExact(checkout, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedCheckoutDate))
                {
                    roomList = roomList.Where(room =>
                        room.Location.Contains(location) &&
                        room.Checking.Date <= parsedCheckingDate.Date &&
                        room.Checkout.Date >= parsedCheckoutDate.Date &&
                        room.NumberOfBeds == numberOfBeds);
                }
            }
            var rooms = await roomList.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = $"Location: {location}, Checking: {checking}, Checkout: {checkout}, Beds: {numberOfBeds}";
            return View("Index", rooms);
        }

        private async Task<bool> RoomsExists(int id)
        {
            return await _db.Rooms.AnyAsync(r => r.RoomId == id);
        }
    }
}

