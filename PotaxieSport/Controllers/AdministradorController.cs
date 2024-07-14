using Microsoft.AspNetCore.Mvc;
using PotaxieSport.Data;
using PotaxieSport.Models;
using System.Diagnostics;
using PotaxieSport.Data.Servicios;
using Microsoft.AspNetCore.Authorization;

namespace PotaxieSport.Controllers
{
    public class AdministradorController : Controller
    {

        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        private readonly ILogger<HomeController> _logger;

        public AdministradorController(ILogger<HomeController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
        }
        [Authorize(Roles = "administrador")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Administrador  ()
        {
            var administrador = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 2).ToList();
            return View("~/Views/Administrador/Administrador/index.cshtml", administrador);
        }
        public IActionResult Coach()
        {
            var coach = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 5).ToList();
            return View("~/Views/Administrador/Coach/index.cshtml", coach);
        }
        public IActionResult Arbitro()
        {
            var arbitro = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 4).ToList();
            return View("~/Views/Administrador/Arbitro/index.cshtml", arbitro);
        }
        public IActionResult Contador()
        {
            var contador = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 3).ToList();
            return View("~/Views/Administrador/Contador/index.cshtml", contador);
        }
        public IActionResult Doctor ()
        {
            var doctor = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 1).ToList();
            return View("~/Views/Administrador/Doctor/index.cshtml", doctor);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
