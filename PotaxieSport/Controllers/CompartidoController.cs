using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Npgsql;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using PotaxieSport.Models.ViewModels;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace PotaxieSport.Controllers
{
    public class CompartidoController : Controller
    {

        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        private readonly TorneoServicio _torneoServicio;

        public CompartidoController(Contexto contexto)
        {
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
            _torneoServicio = new TorneoServicio(contexto);
        }


        [Authorize]
        public IActionResult Torneos()
        {
            // Obtener los claims del usuario autenticado
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

            if (roleClaim != null)
            {
                switch (roleClaim)
                {
                    case "administrador":
                        torneos = _generalServicio.ObtenerTorneos();
                        break;
                    case "doctor":
                        torneos = _generalServicio.ObtenerTorneos().Where(t => t.DoctorId == idUser).ToList();
                        ViewBag.Torneos = torneos;
                        break;
                    case "contador":
                        torneos = _generalServicio.ObtenerTorneos().Where(t => t.ContadorId == idUser).ToList();
                        ViewBag.Torneos = torneos;
                        break;
                    // Puedes agregar más casos para otros roles aquí
                    default:
                        return RedirectToAction("CerrarSesion", "Home");
                }

            }
            ViewBag.Torneos = torneos;
            return View();
        }

        public IActionResult Informacion(int torneoId)
        {

            var claims = User.Claims;
            var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

            var model = _torneoServicio.ObtenerTorneo(torneoId);
            if (model.equiposNoInscritos != null)
            {
                List<SelectListItem> equiposNoInscritos = model.equiposNoInscritos.Select(r => new SelectListItem
                {
                    Value = r.EquipoId.ToString(),
                    Text = r.EquipoNombre
                }).ToList();
                ViewBag.EquiposNoInscritos = equiposNoInscritos;
            }
            switch (roleClaim)
            {
                case "administrador":
                    ViewBag.NumeroPestana = 1;
                    break;
                case "doctor":
                    ViewBag.NumeroPestana = 6;
                    break;
                case "contador":
                    ViewBag.NumeroPestana = 5;
                    break;
                // Puedes agregar más casos para otros roles aquí
                default:
                    return RedirectToAction("CerrarSesion", "Home");
            }
            
                ViewBag.NumeroPestana = 1;
            return View(model);
        }
        [HttpGet]
        public IActionResult AgregarTorneo()
        {
            var categorias = _generalServicio.ObtenerCategorias();
            var administradores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 2).OrderByDescending(u => u.UsuarioId).ToList();
            var contadores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 3).OrderByDescending(u => u.UsuarioId).ToList();
            var doctores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 1).OrderByDescending(u => u.UsuarioId).ToList();

            ViewBag.Categorias = new SelectList(categorias, "CategoriaId", "CategoriaNombre");
            ViewBag.Administradores = new SelectList(administradores, "UsuarioId", "NombreCompleto");
            ViewBag.Contadores = new SelectList(contadores, "UsuarioId", "NombreCompleto");
            ViewBag.Doctores = new SelectList(doctores, "UsuarioId", "NombreCompleto");

            var viewModel = new Torneo();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AgregarTorneo(Torneo torneo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Llamada correcta al método CrearTorneo
                    _generalServicio.CrearTorneo(
                        torneo.NombreTorneo,
                        torneo.CategoriaId,
                        torneo.Genero,
                        torneo.Logo,
                        torneo.AdministradorId,
                        torneo.ContadorId,
                        torneo.DoctorId,
                        torneo.FechaInicio,
                        torneo.FechaFin
                    );

                    return RedirectToAction("Index"); // Cambia la redirección según sea necesario
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(torneo);

        }




    }
}
