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
    public class CarController : Controller

    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<FlightsController> _logger;
        private readonly ISessionService _sessionService;
        public CarController(ApplicationDbContext db, ILogger<FlightsController> logger, ISessionService sessionService)
        {
            _db = db;
            _logger = logger;
            _sessionService = sessionService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Calling car index action");
            try
            {
                var cars = await _db.Cars.ToListAsync();
                _logger.LogInformation(" Hello Hello");
                ViewBag.flightList = cars;
                return View(cars);
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
            _logger.LogInformation("Calling car detail action");
            var car = await _db.Cars.FirstOrDefaultAsync(c => c.CarId == id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
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
        public async Task<IActionResult> Create(Cars car)
        {
            if (ModelState.IsValid)
            {
                await _db.Cars.AddAsync(car);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _db.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId, CarCompany, Model, OrigenLocation, DestinationLocation, Pricing, DepartureTime, ArrivalTime, LimitPassengers, Available, Url")] Cars car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(car);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CarsExists(car.CarId))
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
            return View(car);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _db.Cars.FirstOrDefaultAsync(c => c.CarId == id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPost("DeleteConfirmed/{id:int})"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CarId)
        {
            var car = await _db.Cars.FindAsync(CarId);
            if (car != null)
            {
                _db.Cars.Remove(car);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }



        [HttpGet("Search")]
        public async Task<IActionResult> Search(string pickupLocation, string dropOffLocation, string rentaldate)
        {
            var carList = from c in _db.Cars select c;
            bool searchPerformed = !string.IsNullOrEmpty(pickupLocation) && !string.IsNullOrEmpty(dropOffLocation) && !string.IsNullOrEmpty(dropOffLocation);
            if (searchPerformed)
            {
                DateTime parsedrentaldate;
                if (DateTime.TryParseExact(rentaldate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedrentaldate))
                {
                    // Filter cars on user input
                    carList = carList.Where(f =>
                      f.OrigenLocation.Contains(pickupLocation) &&
                      f.DestinationLocation.Contains(dropOffLocation) &&
                      f.DepartureTime.Date == parsedrentaldate.Date);

                }
            }
            var car = await carList.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = $"pickupLocation: {pickupLocation}, dropOffLocation: {dropOffLocation}, rentaldate: {rentaldate}";
            //display results in Index 
            return View("Index", car);
        }


        private async Task<bool> CarsExists(int id)
        {
            return await _db.Cars.AnyAsync(e => e.CarId == id);
        }
    }
}

