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


        public IActionResult AgregarEquipo(int equipo, int torneo)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM AgregarParticipacion(@equipoId, @torneoId)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@equipoId", equipo);
                        cmd.Parameters.AddWithValue("@torneoId", torneo);

                        cmd.ExecuteReader();  // Ejecuta la consulta
                    }
                }
                return RedirectToAction("Informacion", "Compartido", new { torneoId = torneo });
            }
            catch (Exception ex)
            {
                // Loggear el error (opcional)
                // _logger.LogError(ex, "Error al agregar equipo al torneo");

                // Si deseas mostrar un mensaje de error en la vista
                // TempData["ErrorMessage"] = "Hubo un error al agregar el equipo al torneo.";
                string mensaje = ex.Message;

                return RedirectToAction("Informacion", "Compartido", new { torneoId = torneo });
            }
        }

        public IActionResult EliminarEquipo(int equipo, int torneo)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM EliminarParticipacion(@equipoId, @torneoId)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@equipoId", equipo);
                        cmd.Parameters.AddWithValue("@torneoId", torneo);

                        cmd.ExecuteReader();  // Ejecuta la consulta
                    }
                }
                return RedirectToAction("Informacion", "Compartido", new { torneoId = torneo });
            }
            catch (Exception ex)
            {
                // Loggear el error (opcional)
                // _logger.LogError(ex, "Error al agregar equipo al torneo");

                // Si deseas mostrar un mensaje de error en la vista
                // TempData["ErrorMessage"] = "Hubo un error al agregar el equipo al torneo.";
                string mensaje = ex.Message;

                return RedirectToAction("Informacion", "Compartido", new { torneoId = torneo });
            }
        }



    }
}
