using GBC_Travel_Group_40.Areas.ProductManagement.Models;
using GBC_Travel_Group_40.Data;
using GBC_Travel_Group_40.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace GBC_Travel_Group_40.Areas.ProductManagement.Controllers
{
    [Area("ProductManagement")]
    [Route("[area]/[controller]/[action]")]
    public class CarBookingController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CarBookingController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var bookingC = await _db.CarBookings.ToListAsync();
            return View(bookingC);
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var cb = await _db.CarBookings.FirstOrDefaultAsync(cb => cb.CarBookingId == id);
            if (cb != null)
            {
                ViewBag.car = await _db.Cars.FindAsync(cb.CarId);
                return View(cb);
            }
            int statusCode = 404;
            return RedirectToAction("NotFound", "Home", new { area = "", statusCode });
        }

        [Authorize]
        [HttpGet("Create")]
        public async Task<IActionResult> Create(int fid)
        {
            var car = await _db.Cars.FindAsync(fid);
            if (car != null)
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(userId != null)
                {
                    ViewBag.userId = userId;
                    return View(new CarBookings { CarId = fid, UserId = userId });
                }
                return View(car);
            }
            else
            {
                return RedirectToAction("Index", "Car");
            }
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarBookings bookingC)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _db.CarBookings.AddAsync(bookingC);
                    var car = await _db.Cars.FindAsync(bookingC.CarId);
                    await _db.SaveChangesAsync();
                    if (car != null)
                    {
                        _db.Update(car);
                        // add to the cart
                        var item = new Cart
                        {
                            ProductType = "Car",
                            CarId = bookingC.CarBookingId,
                            Price = car.Pricing,
                            UserId = bookingC.UserId
                        };
                        await _db.Carts.AddAsync(item);
                        await _db.SaveChangesAsync();
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
            var cb = await _db.CarBookings.FindAsync(id);
            if (cb == null)
            {
                return NotFound();
            }
            return View(cb);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarBookingId, CarId, NameOfHolder, PhoneOfHolder, UserId")] CarBookings cb)
        {
                if (id != cb.CarBookingId)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        _db.Update(cb);
                        await _db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await CarBookingExists(cb.CarBookingId))
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
                return View(cb);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cb = await _db.CarBookings.FirstOrDefaultAsync(cb => cb.CarBookingId == id);
            if (cb == null)
            {
                return NotFound();
            }
            return View(cb);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("DeleteConfirmed/{id:int}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CarBookingId)
        {
            try
            {
                var cb = await _db.CarBookings.FindAsync(CarBookingId);
                if (cb != null)
                {
                    var cart = await _db.Carts.FirstOrDefaultAsync(c => c.CarId == CarBookingId);
                    if (cart != null) { _db.Carts.Remove(cart); }
                    _db.CarBookings.Remove(cb);
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

        private async Task<bool> CarBookingExists(int id)
        {
            return await _db.CarBookings.AnyAsync(c => c.CarBookingId == id);
        }


    }
}
