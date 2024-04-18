using GBC_Travel_Group_40.Data;
using GBC_Travel_Group_40.Areas.ProductManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using GBC_Travel_Group_40.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GBC_Travel_Group_40.Models;
using System.Security.Claims;

namespace GBC_Travel_Group_40.Areas.ProductManagement.Controllers
{
    [Area("ProductManagement")]
    [Route("[area]/[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<FlightsController> _logger;
        private readonly ISessionService _sessionService;
        public CartController(ApplicationDbContext db, ILogger<FlightsController> logger, ISessionService sessionService)
        {
            _db = db;
            _logger = logger;
            _sessionService = sessionService;
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null) {
                ViewBag.userId = userId;
                var cartList = await _db.Carts.Where(c => c.UserId == userId).ToListAsync();
                //var cartList = await _db.Carts.ToListAsync();
                return View(cartList);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Carts.FirstOrDefaultAsync(i => i.ProductId == id);
            if (item != null)
            {
                if (item.CarId != null) { ViewBag.car = await _db.Cars.FindAsync(item.CarId); }
                if (item.FlightId != null) { ViewBag.flight = await _db.Flights.FindAsync(item.FlightId); }
                if (item.RoomId != null) { ViewBag.room = await _db.Rooms.FindAsync(item.RoomId); }
                return View(item);
            }
                int statusCode = 404;
                return RedirectToAction("NotFound", "Home", new { area = "", statusCode });
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var item = _db.Carts.Find(id);
                if (item != null)
                {
                    var cb = await _db.CarBookings.FindAsync(item.CarId);
                    if (cb != null) { _db.CarBookings.Remove(cb); }

                    var fb = await _db.BookingFlights.FindAsync(item.FlightId);
                    if (fb != null) { _db.BookingFlights.Remove(fb); }

                    var rb = await _db.RoomBookings.FindAsync(item.RoomId);
                    if (rb != null) { _db.RoomBookings.Remove(rb); }

                    _db.Carts.Remove(item);
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

        [Authorize]
        [HttpGet("Payment")]
        public IActionResult Payment()
        {
            return View();
        }

        [Authorize]
        [HttpPost("Payment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Paymentconfirmed()
        {
            if (ModelState.IsValid)
            {
            }

            return View();
        }
    }
}