using Microsoft.AspNetCore.Mvc;
using PotaxieSport.Data;
using PotaxieSport.Models;
using System.Diagnostics;
using PotaxieSport.Data.Servicios;
using Microsoft.AspNetCore.Authorization;
using System.Numerics;

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

        public IActionResult Administradores()
        {
            var administradores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 2).ToList(); 
            return View(administradores);
        }
        public IActionResult Coachs()
        {
            var coachs = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 5).ToList();
            return View(coachs);
        }
        public IActionResult Doctores()
        {
            var doctores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 1).ToList();
            return View(doctores);
        }

        public IActionResult Contadores()
        {
            var contadores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 3).ToList();
            return View(contadores);
        }

        public IActionResult Arbitros()
        {
            var arbitros = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 4).ToList();
            return View(arbitros);
        }

        public IActionResult EquiposBenjamines()
        {
            var EquiposBenjamines = _generalServicio.ObtenerEquipos().Where(e => e.Categoria == "Benjamines").ToList();
            return View(EquiposBenjamines);
        }

        public IActionResult EquiposInfantiles()
        {
            var equipos = _generalServicio.ObtenerEquipos().Where(e => e.Categoria == "Infantiles").ToList();
            return View(equipos);
        }

        public IActionResult EquiposJuveniles()
        {
            var equipos = _generalServicio.ObtenerEquipos().Where(e => e.Categoria == "Juveniles").ToList();
            return View(equipos);
        }

    }
}
