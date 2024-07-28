using Microsoft.AspNetCore.Mvc;
using PotaxieSport.Data;
using PotaxieSport.Models;
using System.Diagnostics;
using PotaxieSport.Data.Servicios;
using Microsoft.AspNetCore.Authorization;
using System.Numerics;
using System;
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

        [HttpGet]
        public IActionResult AgregarUsuario(int idRo, string Roll, string url)
        {
            ViewBag.IdRo = idRo;
            ViewBag.Roll = Roll;
            ViewBag.url = url;
            return View();
        }
      
      [HttpPost]
        public IActionResult CrearUsuario(Usuario model, string url) 
        {
            ViewBag.url = url;

            if (ModelState.IsValid)
            {
                // Asumimos que error_autentificacion inicia en 0
                model.ErrorAutentificacion = 0;
                #pragma warning disable CS8604 // Posible argumento de referencia nulo
                _generalServicio.CrearUsuario(model.Nombre, model.ApPaterno, model.ApMaterno, model.Username, model.Email, model.RolId, model.ErrorAutentificacion, model.Password);
                return RedirectToAction(url); // Cambia "Index" por la acción a la que deseas redirigir después de crear el usuario.
            }

            return View("AgregarUsuario", model);
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
