using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Npgsql;
using PotaxieSport.Models.ViewModels;
using System.Data;

namespace PotaxieSport.Controllers
{
    public class CoachController : Controller
    {
        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        private readonly ILogger<HomeController> _logger;

        public CoachController(ILogger<HomeController> logger, Contexto contexto)
        {
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
            _logger = logger;
        }
        [Authorize(Roles = "coach")]
        public IActionResult Index()

        {

            var claims = User.Claims;

            var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

            // Buscar idUsuario
            var idUserClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value ?? string.Empty;
            int idUser;
            if (!int.TryParse(idUserClaim, out idUser))
            {
                idUser = -1; // Valor predeterminado si la conversión falla
            }

            ViewBag.Id = idUser;
            var torneos = _generalServicio.ObtenerTorneosPorCoach(idUser);
            var coach = _generalServicio.ObtenerUsuarioPorId(idUser);
            ViewBag.Torneos = torneos;
            ViewBag.Coach = coach;

            return View();
        }



        public ActionResult Informacion(int torneoId, int userId)
        {
            var equipo = _generalServicio.GetEquipoByCoachAndTorneo(torneoId, userId);
           
            var coach = _generalServicio.ObtenerUsuarioPorId(userId);
            var categoria = _generalServicio.ObtenerCategoriaPorId(equipo.CategoriaId);
            var detallesPartido = _generalServicio.ObtenerDetallesPartido(equipo.EquipoId);


            ViewBag.Equiponame = equipo.EquipoNombre;
            ViewBag.Coach = coach;
            ViewBag.Categoria = categoria;
            ViewBag.Id = userId;

            return View(detallesPartido);
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
