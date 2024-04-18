using GBC_Travel_Group_40.Data;
using GBC_Travel_Group_40.Areas.ProductManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using GBC_Travel_Group_40.Services;
using Microsoft.AspNetCore.Authorization;


namespace GBC_Travel_Group_40.Areas.ProductManagement.Controllers
{
    [Area("ProductManagement")]
    [Route("[area]/[controller]/[action]")]

    public class FlightsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<FlightsController> _logger;
        private readonly ISessionService _sessionService;
        public FlightsController(ApplicationDbContext db, ILogger<FlightsController> logger, ISessionService sessionService)
        {
            _db = db;
            _logger = logger;
            _sessionService = sessionService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int? numPassengers)
        {

            ViewData["numPassengers"] = numPassengers;
            _logger.LogInformation("Calling flight index action");
            try
            {
                var flights = await _db.Flights.ToListAsync();
                var value = _sessionService.GetSessionData<int?>("Visited") ?? 0;
                _sessionService.SetSessionData("Visited", value + 1);

                ViewBag.mysession = value + 1;

                _logger.LogInformation(" Hello Hello");
                ViewBag.flightList = flights;
                return View(flights);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(null);
            }
        }

        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Calling flight detail action");
            var flight = await _db.Flights.FirstOrDefaultAsync(f => f.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
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
        public async Task<IActionResult> Create(Flights flight)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _db.Flights.AddAsync(flight);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(flight);
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
            var flight = await _db.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightId, Airlines, Origen, Destination, MaxSeat, NumOfPassengers, DepartureTime, ArrivalTime, Price")] Flights flight)
        {
            if (id != flight.FlightId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(flight);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await FlightsExists(flight.FlightId))
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
            return View(flight);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var flight = await _db.Flights.FirstOrDefaultAsync(f => f.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("DeleteConfirmed/{id:int})"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int FlightId)
        {
            try
            {
                var flight = await _db.Flights.FindAsync(FlightId);
                if (flight != null)
                {
                    _db.Flights.Remove(flight);
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
        public async Task<IActionResult> Search(string leavingFrom, string goingTo, string departureDate, string numPassengers)
        {
            var flightList = from f in _db.Flights select f;
            bool searchPerformed = !string.IsNullOrEmpty(goingTo) && !string.IsNullOrEmpty(leavingFrom) && !string.IsNullOrEmpty(departureDate);
            int parsedPassengers = int.Parse(numPassengers);

            if (searchPerformed)
            {
                DateTime parsedDepartureDate;
                if (DateTime.TryParseExact(departureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDepartureDate))
                {
                    // Filter flights on user input
                    flightList = flightList.Where(f =>
                      f.Origen.Contains(leavingFrom) &&
                      f.Destination.Contains(goingTo) &&
                      f.DepartureTime.Date == parsedDepartureDate.Date &&
                      f.NumOfPassengers + parsedPassengers <= f.MaxSeat);
                }
            }
            var flights = await flightList.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = $"Leaving: {leavingFrom}, Going: {goingTo}, Departure Date: {departureDate}, Passengers: {numPassengers}";
            //display results in Index 
            return View("Index", flights);
        }

        private async Task<bool> FlightsExists(int id)
        {
            return await _db.Flights.AnyAsync(e => e.FlightId == id);
        }
    }
}
