using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using Npgsql;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using PotaxieSport.Models.ViewModels;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using System.Xml.Linq;

namespace PotaxieSport.Controllers
{
    public class CompartidoController : Controller
    {

        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        private readonly TorneoServicio _torneoServicio;
        private readonly ArchivosServicio _archivosServicio;

        public CompartidoController(Contexto contexto, IWebHostEnvironment hostingEnvironment)
        {
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
            _torneoServicio = new TorneoServicio(contexto);
            _archivosServicio = new ArchivosServicio(contexto, hostingEnvironment);
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
            if(model.equipos != null)
            {
                List<SelectListItem> equipos = model.equipos.Select(r => new SelectListItem
                {
                    Value = r.EquipoId.ToString(),
                    Text = r.EquipoNombre
                }).ToList();
                ViewBag.Equipos = equipos;
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

        //[HttpPost]
        /*public IActionResult AgregarTorneo(Torneo torneo)
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

                    return RedirectToAction("Index"); 
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(torneo);

        }*/

        public IActionResult ConsultarEquipos(int torneoId)
        {
            var model = _torneoServicio.ObtenerEquipos().Where(e => e.TorneoActualId == torneoId).ToList();
            
            return Json(model);
        }


        /*----------------------------- Agregar Equipos --------------------------------------*/

        public IActionResult AgregarEquipo(int torneo, int equipo)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                try
                {
                    using (var cmd = new NpgsqlCommand("SELECT  AgregarParticipacion(@equipoId, @torneoId)", connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("equipoId", equipo);
                        cmd.Parameters.AddWithValue("@torneoId", torneo);

                        cmd.ExecuteNonQuery();

                        // Redirigir después de completar la operación
                        return RedirectToAction("Informacion", "Compartido", new { torneoId = torneo });
                    }
                }
                catch (PostgresException ex)
                {
                    // Manejo de excepciones en caso de error
                    TempData["ErrorMessage"] = "Error al insertar el partido: " + ex.Message;
                    return RedirectToAction("Error", "Compartido", new { torneoId = torneo });
                }
            }
        }

        public IActionResult EliminarEquipo(int torneo, int equipo)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                try
                {
                    using (var cmd = new NpgsqlCommand("SELECT  EliminarParticipacion(@equipoId, @torneoId)", connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("equipoId", equipo);
                        cmd.Parameters.AddWithValue("@torneoId", torneo);

                        cmd.ExecuteNonQuery();

                        // Redirigir después de completar la operación
                        return RedirectToAction("Informacion", "Compartido", new { torneoId = torneo });
                    }
                }
                catch (PostgresException ex)
                {
                    // Manejo de excepciones en caso de error
                    TempData["ErrorMessage"] = "Error al insertar el partido: " + ex.Message;
                    return RedirectToAction("Error", "Compartido", new { torneoId = torneo });
                }
            }
        }


        /*----------------------------- Formulario Partidos --------------------------------------*/

        [Authorize]
        public IActionResult ObtenerSubSubEquipos(int torneoId, int equipoId)
        {
            // Realiza la consulta a la base de datos para obtener las subcategorías según la categoría seleccionada
            var subsubcategorias = _torneoServicio.ObtenerEquipos().Where(e => e.TorneoActualId == torneoId && e.EquipoId != equipoId).ToList();

            // Transforma el objeto en JSON
            var jsonResult = JsonConvert.SerializeObject(subsubcategorias);

            // Devuelve el resultado como una respuesta JSON
            return Content(jsonResult, "application/json");
        }

        [Authorize]
        public IActionResult ObtenerSubArbitros(DateTime fecha, TimeSpan hora)
        {

            // Obtenemos el día de la semana
            DayOfWeek diaSemana = fecha.DayOfWeek;

            // Convertimos el día a un string en español
            string nombreDia = ObtenerNombreDia(diaSemana);

            // Realiza la consulta a la base de datos para obtener las subcategorías según la categoría seleccionada
            var subsubcategorias = _torneoServicio.ObtenerArbitros(nombreDia, hora);

            // Transforma el objeto en JSON
            var jsonResult = JsonConvert.SerializeObject(subsubcategorias);

            // Devuelve el resultado como una respuesta JSON
            return Content(jsonResult, "application/json");
        }

        // Método para convertir el DayOfWeek a un string en español
        public static string ObtenerNombreDia(DayOfWeek dia)
        {
            switch (dia)
            {
                case DayOfWeek.Monday:
                    return "Lunes";
                case DayOfWeek.Tuesday:
                    return "Martes";
                case DayOfWeek.Wednesday:
                    return "Miércoles";
                case DayOfWeek.Thursday:
                    return "Jueves";
                case DayOfWeek.Friday:
                    return "Viernes";
                case DayOfWeek.Saturday:
                    return "Sábado";
                case DayOfWeek.Sunday:
                    return "Domingo";
                default:
                    return "Día desconocido";
            }
        }


        /*------------------------------------ Crear Partidos  ---------------------------------*/
        public IActionResult PartidosCreate(int torneoId, int equipoVisitante, int equipoDefensor, string lugar, DateTime fecha, TimeSpan hora, int arbitro, decimal costo)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                try
                {
                    using (var cmd = new NpgsqlCommand("SELECT insertar_partido(@p_torneo, @p_equipoV, @p_equipoD, @p_lugar, @p_fecha, @p_hora, @p_arbitro, @p_costo)", connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("p_torneo", torneoId);
                        cmd.Parameters.AddWithValue("p_equipoV", equipoVisitante);
                        cmd.Parameters.AddWithValue("p_equipoD", equipoDefensor);
                        cmd.Parameters.AddWithValue("p_lugar", lugar);
                        cmd.Parameters.AddWithValue("p_fecha", fecha);
                        cmd.Parameters.AddWithValue("p_hora", hora);
                        cmd.Parameters.AddWithValue("p_arbitro", arbitro);
                        cmd.Parameters.AddWithValue("p_costo", costo);

                        cmd.ExecuteNonQuery();

                        // Redirigir después de completar la operación
                        return RedirectToAction("Informacion", "Compartido", new { torneoId = torneoId });
                    }
                }
                catch (PostgresException ex)
                {
                    // Manejo de excepciones en caso de error
                    TempData["ErrorMessage"] = "Error al insertar el partido: " + ex.Message;
                    return RedirectToAction("Error", "Compartido", new { torneoId = torneoId });
                }
            }
        }


        /*----------------------- Movimientos Económicos ----------------------------------*/
        public IActionResult TransaccionCreate(int torneo, int usuario, string tipoM, decimal cantidad, IFormFile comprobante)
        {
            DateTime fechaActual = DateTime.Now.Date;
            if (comprobante != null && comprobante.Length > 0)
            {
                //Asigna nombre al archivo con la estructura tipo_id_nombre.dominio  (El remplace quita los espacios)
                string nombre = comprobante.FileName.Replace(" ", "");

                //Llamar a la función que sube el archivo a las carpetas de ASP.NET
                string respuesta = _archivosServicio.SubirArchivo(comprobante, nombre, "Comprobante");


                // Aquí puedes llamar a un procedimiento almacenado para registrar la información en la base de datos
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    try
                    {
                        using (var cmd = new NpgsqlCommand("SELECT insertar_transaccion(@p_torneo, @p_usuario, @p_tipo, @p_cantidad, @p_fecha, @p_comprobante)", connection))
                        {
                            cmd.CommandType = CommandType.Text;

                            cmd.Parameters.AddWithValue("p_torneo", torneo);
                            cmd.Parameters.AddWithValue("p_usuario", usuario);
                            cmd.Parameters.AddWithValue("p_tipo", tipoM);
                            cmd.Parameters.AddWithValue("p_cantidad", cantidad);
                            cmd.Parameters.AddWithValue("p_fecha", fechaActual);
                            cmd.Parameters.AddWithValue("p_comprobante", nombre);

                            cmd.ExecuteNonQuery();

                            // Redirigir después de completar la operación
                            return RedirectToAction("Informacion", "Compartido", new { torneoId = torneo });
                        }
                    }
                    catch (PostgresException ex)
                    {
                        // Manejo de excepciones en caso de error
                        TempData["ErrorMessage"] = "Error al insertar el partido: " + ex.Message;
                        return RedirectToAction("Error", "Compartido", new { torneoId = torneo });
                    }
                }
            }

            // Si no se sube ningún archivo, puedes manejar el error aquí
            return RedirectToAction("Informacion", "Compartido", new { torneoId = torneo });
            
        }


        public IActionResult SubirComprobantePago(int torneo, int pago, IFormFile archivoPago)
        {
            DateTime fechaActual = DateTime.Now.Date;
            if (archivoPago != null && archivoPago.Length > 0)
            {
                //Asigna nombre al archivo con la estructura tipo_id_nombre.dominio  (El remplace quita los espacios)
                string nombre = archivoPago.FileName.Replace(" ", "");

                //Llamar a la función que sube el archivo a las carpetas de ASP.NET
                string respuesta = _archivosServicio.SubirArchivo(archivoPago, nombre, "Pago");

                _archivosServicio.GuardarArchivoFotoEnBD(nombre, pago, "Pago");
            }

            // Si no se sube ningún archivo, puedes manejar el error aquí
            return RedirectToAction("Informacion", "Compartido", new { torneoId = torneo });

        }
        public IActionResult Crear()
        {
            var categorias = _generalServicio.ObtenerCategorias();
            var administradores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 2).OrderByDescending(u => u.UsuarioId).ToList();
            var contadores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 3).OrderByDescending(u => u.UsuarioId).ToList();
            var doctores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 1).OrderByDescending(u => u.UsuarioId).ToList();

            ViewBag.Categorias = new SelectList(categorias, "CategoriaId", "CategoriaNombre");
            ViewBag.Administradores = new SelectList(administradores, "UsuarioId", "NombreCompleto");
            ViewBag.Contadores = new SelectList(contadores, "UsuarioId", "NombreCompleto");
            ViewBag.Doctores = new SelectList(doctores, "UsuarioId", "NombreCompleto");

            return View();
        }



        [HttpPost]
        public IActionResult Crear(Torneo torneo)
        {
            if (ModelState.IsValid)
            {
                int torneoId = _generalServicio.CrearTorneo(torneo);
                TempData["NombreTorneo"] = torneo.NombreTorneo;

                return RedirectToAction("SubirLogo", new { id = torneoId }); // Redirige a una acción para mostrar los detalles del torneo

            }

            // Si el modelo no es válido, volver a cargar los datos y mostrar la vista con errores
            var categorias = _generalServicio.ObtenerCategorias();
            var administradores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 2).OrderByDescending(u => u.UsuarioId).ToList();
            var contadores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 3).OrderByDescending(u => u.UsuarioId).ToList();
            var doctores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 1).OrderByDescending(u => u.UsuarioId).ToList();

            ViewBag.Categorias = new SelectList(categorias, "CategoriaId", "CategoriaNombre");
            ViewBag.Administradores = new SelectList(administradores, "UsuarioId", "NombreCompleto");
            ViewBag.Contadores = new SelectList(contadores, "UsuarioId", "NombreCompleto");
            ViewBag.Doctores = new SelectList(doctores, "UsuarioId", "NombreCompleto");

            return View(torneo); // Reenviar el modelo con errores a la vista
        }


        public IActionResult SubirLogo(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public IActionResult SubirLogo(string? archivoError, int id, IFormFile file, string tipo)
        {
            if (file != null && file.Length > 0)
            {
                if (tipo != null)
                {
                    //Asigna nombre al archivo con la estructura tipo_id_nombre.dominio  (El remplace quita los espacios)
                    string nombre = tipo + "_" + id + "_" + file.FileName.Replace(" ", "");

                    //Llamar a la función que sube el archivo a las carpetas de ASP.NET
                    string respuesta = _archivosServicio.SubirArchivo(file, nombre, tipo);

                    //Subir el archivo a la base de datos.
                    _archivosServicio.GuardarArchivoFotoEnBD(nombre, id, tipo);


                    return RedirectToAction("Torneos", "Compartido", new { archivoError = respuesta }); //{ archivoError = "Archivo subido con éxito" });
                }
                else
                {
                    return RedirectToAction("SubirImagenes", "Administrador", new { archivoError = "Por favor, selecciona un tipo de archivo válido." });
                }


            }
            else
            {
                return RedirectToAction("SubirImagenes", "Administrador", new { archivoError = "Por favor, selecciona un archivo válido." });
            }

        }
    }
}
