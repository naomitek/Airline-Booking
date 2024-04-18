using GBC_Travel_Group_40.Data;
using GBC_Travel_Group_40.Areas.ProductManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GBC_Travel_Group_40.Areas.ProductManagement.Controllers
{
    [Area("ProductManagement")]
    [Route("[area]/[controller]/[action]")]
    public class PassengersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PassengersController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var passengers = await _db.Passengers.ToListAsync();
            return View(passengers);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(int fid)
        {
            ViewBag.fid = fid;
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Passengers passenger, int fid)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _db.Passengers.AddAsync(passenger);
                    await _db.SaveChangesAsync();
                    ViewBag.pid = passenger.PassengerId;
                    return RedirectToAction("Create", "BookingFlights", new { pid = passenger.PassengerId, fid });
                }
                return View(passenger);
            }
            catch
            {
                string type = "booking";
                return RedirectToAction("FailUpdataDb", "Home", new { area = "", type });
            }
        }
    }
}