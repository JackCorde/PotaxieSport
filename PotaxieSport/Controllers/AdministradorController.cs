using Microsoft.AspNetCore.Mvc;
using PotaxieSport.Data;
using PotaxieSport.Models;
using System.Diagnostics;
using PotaxieSport.Data.Servicios;
using Microsoft.AspNetCore.Authorization;
using System.Numerics;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data;
using Microsoft.Extensions.Hosting.Internal;
using Npgsql;
using Microsoft.IdentityModel.Tokens;


namespace PotaxieSport.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        private readonly ILogger<HomeController> _logger;
        private readonly PathServicio _PathServicio;

        public AdministradorController(ILogger<HomeController> logger, Contexto contexto, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
            _PathServicio = new PathServicio(hostingEnvironment, contexto);
        }

        [Authorize(Roles = "administrador")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Administradores()
        {
            var administradores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 2)
            .OrderByDescending(u => u.UsuarioId) 
            .ToList();

            return View(administradores);
        }
        public IActionResult Coachs()
        {
            var coachs = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 5)
            .OrderByDescending(u => u.UsuarioId)
            .ToList();
            return View(coachs);
        }
        public IActionResult Doctores()
        {
            var doctores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 1).OrderByDescending(u => u.UsuarioId).ToList();
            return View(doctores);
        }

        public IActionResult Contadores()
        {
            var contadores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 3).OrderByDescending(u => u.UsuarioId).ToList();
            return View(contadores);
        }

        public IActionResult Arbitros()
        {
            var arbitros = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 4).OrderByDescending(u => u.UsuarioId).ToList();
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

        [HttpGet]
        public IActionResult AgregarUsuario(string url)
        {
            // Asignar el parámetro url a ViewBag
            ViewBag.url = url;

            var roles = _generalServicio.ObtenerRoles();
            ViewBag.Rol = new SelectList(roles, "RolId", "RolNombre");

            return View();
        }

        [HttpPost]
        public IActionResult CrearUsuario(Usuario model, string url)
        {
            var roles = _generalServicio.ObtenerRoles();
            ViewBag.Rol = new SelectList(roles, "RolId", "RolNombre");
            ViewBag.Url = url;

            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si el correo ya existe
                    if (_generalServicio.EmailExists(model.Email))
                    {
                        ModelState.AddModelError("Email", "El correo electrónico ya existe.");
                        return View("AgregarUsuario", model); // Retorna la vista con el mensaje de error
                    }
                    model.ErrorAutentificacion = 0;
                    _generalServicio.CrearUsuario(model.Nombre, model.ApPaterno, model.ApMaterno, model.Username, model.Email, model.RolId, model.ErrorAutentificacion, model.Password);
                    TempData["SuccessMessage"] = "Usuario registrado exitosamente.";
                    if (string.IsNullOrEmpty(url))
                    {
                        url = "Index";
                    }
                    return RedirectToAction(url); // Redirige a la acción especificada
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al crear el usuario: " + ex.Message);
                    return View("AgregarUsuario", model); // Retorna la vista con el mensaje de error
                }
            }
            return View("AgregarUsuario", model);
        }

        // Acción para mostrar el formulario de actualización
        [HttpGet]
        public IActionResult ActualizarUsuario(int id)
        {
            // Obtener el usuario por ID
            var usuario = _generalServicio.ObtenerUsuarioPorId(id);

            if (usuario == null)
            {
                return NotFound(); // O redirigir a una página de error
            }

            // Cargar roles para el dropdown
            var roles = _generalServicio.ObtenerRoles();
            ViewBag.Rol = new SelectList(roles, "RolId", "RolNombre");

            // Pasar el usuario a la vista
            return View(usuario);
        }


        [HttpPost]
        [HttpPost]
        public IActionResult ActualizarUsuario(Usuario model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _generalServicio.ActualizarUsuario(
                        model.UsuarioId,
                        model.Nombre,
                        model.ApPaterno,
                        model.ApMaterno,
                        model.Username,
                        model.Email,
                        model.RolId
                    );

                    TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
                    return RedirectToAction("Index"); // Redirige a la lista de administradores o donde sea necesario
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar el usuario: " + ex.Message);
                }
            }

            // Si el modelo no es válido o hubo un error, regresa a la vista con el modelo
            return View(model);
        }

        public IActionResult SubirImagenes(string? archivoError, int? id)
        {
            if (archivoError != null)
            {
                ViewBag.Error=archivoError;
            }
            return View();
        }




        // ... Código previo ...
        [Authorize]
        [RequestSizeLimit(200 * 1024 * 1024)]
        public IActionResult SubirImagenTorneo(IFormFile file, int torneoId)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = _PathServicio.PathTorneoImagen();

                var filePath = Path.Combine(uploadsFolder, torneoId + "T_" + file.FileName);

                // Verificar si el archivo con el mismo nombre ya existe
                if (System.IO.File.Exists(filePath))
                {
                    return RedirectToAction("Index", "Administrador", new { id = torneoId, archivoError = "Ya existe un archivo con este nombre." });
                }


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }


                // Guardar información en la base de datos
                GuardarImagenTorneoEnBD(torneoId + "T_" + file.FileName, torneoId);
                return RedirectToAction("Index", "Administrador", new { id = torneoId, archivoError = "Archivo subido correctamente." });
            }
            else
            {
                return RedirectToAction("Index", "Administrador", new { id = torneoId, archivoError = "Por favor, selecciona un archivo válido." });
            }


        }

        [Authorize]
        private void GuardarImagenTorneoEnBD(string nombreArchivo, int torneoId)
        {

            using (NpgsqlConnection con = new(_contexto.Conexion))
            {

                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM cargarImagenTorneo(@torneoId, @nombre)", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@torneoId", torneoId);
                    cmd.Parameters.AddWithValue("@nombre", nombreArchivo);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

        }

    }
}
