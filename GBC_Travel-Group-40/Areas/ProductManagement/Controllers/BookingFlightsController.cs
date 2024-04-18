using GBC_Travel_Group_40.Data;
using GBC_Travel_Group_40.Areas.ProductManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SendGrid.Helpers.Mail;
using System.Security.Claims;

namespace GBC_Travel_Group_40.Areas.ProductManagement.Controllers
{
    [Area("ProductManagement")]
    [Route("[area]/[controller]/[action]")]
    public class BookingFlightsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BookingFlightsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            //ViewBag.flight = _db.Flights.Find(bookingF.FlightId);
            //ViewBag.passengers = _db.Passengers.Find(bookingF.PassengerId);
            var bookingF = await _db.BookingFlights.ToListAsync();
            return View(bookingF);
        }

        [Authorize]
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var fb = await _db.BookingFlights.FirstOrDefaultAsync(fb => fb.BookingFlightId == id);
            if (fb != null)
            {
                ViewBag.passenger = await _db.Passengers.FindAsync(fb.PassengerId);
                ViewBag.flight = await _db.Flights.FindAsync(fb.FlightId);
                return View(fb);
            }
            int statusCode = 404;
            return RedirectToAction("NotFound", "Home", new { area = "", statusCode });
        }

        [Authorize]
        [HttpGet("MakeBooking")]
        public IActionResult MakeBooking(int fid, int numPassengers)
        {
            ViewBag.fid = fid;
            ViewData["numPassengers"] = numPassengers;
            return View();
        }

        [Authorize]
        [HttpPost("MakeBooking")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeBooking(Passengers passenger, int fid, int numPassengers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _db.Passengers.AddAsync(passenger);
                    await _db.SaveChangesAsync();
                    string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (userId != null)
                    {
                        var bookingF = new BookingFlights
                        {
                            PassengerId = passenger.PassengerId,
                            FlightId = fid,
                            UserId = userId
                        };
                        await _db.BookingFlights.AddAsync(bookingF);
                        await _db.SaveChangesAsync();

                        var flight = await _db.Flights.FindAsync(bookingF.FlightId);
                        flight.NumOfPassengers++;
                        _db.Update(flight);
                        // add to cart
                        var item = new Cart
                        {
                            ProductType = "Flight",
                            FlightId = bookingF.BookingFlightId,
                            Price = flight.Price,
                            UserId = bookingF.UserId
                        };
                        await _db.Carts.AddAsync(item);
                        await _db.SaveChangesAsync();
                        numPassengers--;
                        if (numPassengers > 0)
                        {
                            return RedirectToAction("MakeBooking", new { fid, numPassengers });
                        }
                    }
                    return RedirectToAction("Index", "Cart");
                }
                return View();
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
            var fb = await _db.BookingFlights.FindAsync(id);
            if (fb == null)
            {
                return NotFound();
            }
            ViewBag.passenger = await _db.Passengers.FindAsync(fb.PassengerId);
            return View(fb);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingFlightId, FlightId, PassengerId, UserId")] BookingFlights bf)
        {
            try
            {
                if (id != bf.BookingFlightId)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        _db.Update(bf);
                        await _db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await BookingFlightExists(bf.BookingFlightId))
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
                return View(bf);
            }
            catch
            {
                string type = "update";
                return RedirectToAction("FailUpdataDb", "Home", new { area = "", type });
            }
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fb = await _db.BookingFlights.FirstOrDefaultAsync(fb => fb.BookingFlightId == id);
            if (fb == null)
            {
                return NotFound();
            }
            return View(fb);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("DeleteConfirmed/{id:int})"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int BookingFlightId)
        {
            try
            {
                var fb = await _db.BookingFlights.FindAsync(BookingFlightId);
                if (fb != null)
                {
                    var cart = await _db.Carts.FirstOrDefaultAsync(c => c.FlightId == BookingFlightId);
                    if (cart != null) { _db.Carts.Remove(cart); }
                    _db.BookingFlights.Remove(fb);
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
        private async Task<bool> BookingFlightExists(int id)
        {
            return await _db.BookingFlights.AnyAsync(bf => bf.BookingFlightId == id);
        }

    }
}