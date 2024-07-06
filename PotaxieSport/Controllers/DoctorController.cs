using Microsoft.AspNetCore.Mvc;
using PotaxieSport.Data;
using PotaxieSport.Models;
using System.Diagnostics;
using PotaxieSport.Data.Servicios;
using Microsoft.AspNetCore.Authorization;

namespace PotaxieSport.Controllers
{
    public class DoctorController : Controller
    {
        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        private readonly ILogger<HomeController> _logger;

        public DoctorController(ILogger<HomeController> logger, Contexto contexto)
        {
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
            _logger = logger;
        }

        [Authorize(Roles = "doctor")]
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
