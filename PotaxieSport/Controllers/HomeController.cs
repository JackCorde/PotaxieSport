using Microsoft.AspNetCore.Mvc;
using PotaxieSport.Data;
using PotaxieSport.Models;
using System.Diagnostics;
using PotaxieSport.Data.Servicios;

namespace PotaxieSport.Controllers
{
    public class HomeController : Controller
    {

        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Contexto contexto)
        {

            _logger = logger;
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
        }

        public IActionResult Index()
        {
            var torneos = _generalServicio.ObtenerTorneos();
            ViewBag.Torneos = torneos;
            return View();
        }

        public IActionResult Torneos()
        {
            var torneos = _generalServicio.ObtenerTorneos();
            ViewBag.Torneos = torneos;
            return View();
        }

        public IActionResult Equipos()
        {
            var equipos = _generalServicio.ObtenerEquipos();
            ViewBag.Equipos = equipos;
            return View();
        }

        public IActionResult Partidos()
        {
            var partidos = _generalServicio.ObtenerPartidos();
            ViewBag.Partidos = partidos;
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
