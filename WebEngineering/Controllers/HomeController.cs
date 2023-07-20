using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebEngineering.Data;
using WebEngineering.Models;

namespace WebEngineering.Controllers
{
    public class HomeController : Controller    
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IdentityContext _context;

        public HomeController(ILogger<HomeController> logger, IdentityContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}