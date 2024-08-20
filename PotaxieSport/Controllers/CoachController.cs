using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using System.Diagnostics;
using System.Security.Claims;
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

            List<Torneo> torneos = new();


             torneos = _generalServicio.ObtenerTorneosPorCoach(idUser);

            ViewBag.idUser = idUser;
            ViewBag.Torneos = torneos;

            return View();

        }


        public IActionResult informacion(int torneoId, int idUser)
        {
            var equipo = _generalServicio.GetEquipoByCoachAndTorneo(torneoId, idUser);
            var detallesPartido = _generalServicio.ObtenerDetallesPartido(equipo.EquipoId);
            ViewBag.equipo = equipo.EquipoNombre;

            return View(detallesPartido); // Aquí pasas los detalles a la vista
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
