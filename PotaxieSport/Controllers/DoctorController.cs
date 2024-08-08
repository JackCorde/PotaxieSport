using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using System.Diagnostics;

namespace PotaxieSport.Controllers
{
    public class DoctorController : Controller
    {
        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        private readonly TorneoServicio _torneoServicio;

        public DoctorController(Contexto contexto)
        {
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
            _torneoServicio = new TorneoServicio(contexto);
        }

        [Authorize]
        public IActionResult Index(int equipoId, int torneoId)
        {
            var jugadores = _torneoServicio.JugadoresPorEquipo(equipoId);
            ViewBag.TorneoId = torneoId;
            ViewBag.Jugadores = jugadores;
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
