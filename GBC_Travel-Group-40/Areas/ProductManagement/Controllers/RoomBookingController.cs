using GBC_Travel_Group_40.Areas.ProductManagement.Models;
using GBC_Travel_Group_40.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace GBC_Travel_Group_40.Areas.ProductManagement.Controllers
{
    [Area("ProductManagement")]
    [Route("[area]/[controller]/[action]")]
    public class RoomBookingController : Controller
    {
        private readonly ApplicationDbContext _db;
        public RoomBookingController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {

            var bookingR = await _db.RoomBookings.ToListAsync();
            return View(bookingR);
        }

        [Authorize]
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var rb = await _db.RoomBookings.FirstOrDefaultAsync(rb => rb.RoomBookingId == id);
            if (rb != null)
            {
                ViewBag.room = await _db.Rooms.FindAsync(rb.RoomId);
                return View(rb);
            }
            int statusCode = 404;
            return RedirectToAction("NotFound", "Home", new { area = "", statusCode });
        }

        [Authorize]
        [HttpGet("Create")]
        public async Task<IActionResult> Create(int fid)
        {
            var room = await _db.Rooms.FindAsync(fid);
            if (room != null)
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    ViewBag.fid = fid;
                    return View(new RoomBookings { RoomId = fid, UserId = userId });
                }
                return View(room);
            }
            else
            {

                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomBookings bookingR)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _db.RoomBookings.AddAsync(bookingR);
                    var room = await _db.Rooms.FindAsync(bookingR.RoomId);
                    await _db.SaveChangesAsync();
                    if (room != null)
                    {
                        _db.Update(room);
                        // add to the cart
                        var item = new Cart
                        {
                            ProductType = "Hotel",
                            RoomId = bookingR.RoomBookingId,
                            Price = room.PricePerNight,
                            UserId = bookingR.UserId
                        };
                        await _db.Carts.AddAsync(item);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        // Handle the case when the room is not found
                        // For example, return a not found response or redirect to an error page
                        return NotFound();
                    }
                }
                return RedirectToAction("Index", "Cart");
            }
            catch
            {
                string type = "booking";
                return RedirectToAction("FailUpdataDb", "Home", new { area = "", type });
            }
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var rb = await _db.RoomBookings.FindAsync(id);
            if (rb == null)
            {
                return NotFound();
            }
            return View(rb);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomBookingId, RoomId, NameOfHolder, PhoneOfHolder, UserId")] RoomBookings rb)
        {
            if (id != rb.RoomBookingId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(rb);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await RoomBookingExists(rb.RoomBookingId))
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
            return View(rb);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rb = await _db.RoomBookings.FirstOrDefaultAsync(rb => rb.RoomBookingId == id);
            if (rb == null)
            {
                return NotFound();
            }
            return View(rb);
        }
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("DeleteConfirmed/{id:int}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int RoomBookingId)
        {
            try
            {
                var rb = await _db.RoomBookings.FindAsync(RoomBookingId);
                if (rb != null)
                {
                    var cart = await _db.Carts.FirstOrDefaultAsync(c => c.RoomId == RoomBookingId);
                    if (cart != null) { _db.Carts.Remove(cart); }
                    _db.RoomBookings.Remove(rb);
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

        private async Task<bool> RoomBookingExists(int id)
        {
            return await _db.Cars.AnyAsync(e => e.CarId == id);
        }
    }
}

