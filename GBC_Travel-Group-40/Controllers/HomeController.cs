using GBC_Travel_Group_40.Models;
using GBC_Travel_Group_40.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GBC_Travel_Group_40.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISessionService _sessionService;

        public HomeController(ILogger<HomeController> logger, ISessionService sessionService)
        {
            _logger = logger;
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            //Lab12
            const string sessionKey = "VisitCount";
            int visitCount = _sessionService.GetSessionData<int>(sessionKey);
            visitCount++;
            _sessionService.SetSessionData(sessionKey, visitCount);

            ViewData["VisitCount"] = visitCount;
            
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NotFound(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound");
            }
            if (statusCode == 500)
            {
                return View("InternalServerError");
            }
            return View("Error");
        }

        
    }
}
