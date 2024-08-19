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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
