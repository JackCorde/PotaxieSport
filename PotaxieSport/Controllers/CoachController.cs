using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using System.Diagnostics;

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

            return View();
        }

        
    }
}
