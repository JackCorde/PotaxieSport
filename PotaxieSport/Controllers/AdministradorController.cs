using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using PotaxieSport.Models.ViewModels;
using System.Data;
using System.Threading.Tasks;

namespace PotaxieSport.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        private readonly ArchivosServicio _archivosServicio;
        private readonly ILogger<HomeController> _logger;

        public AdministradorController(ILogger<HomeController> logger, Contexto contexto, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
            _archivosServicio = new ArchivosServicio(contexto, hostingEnvironment);
        }

        [Authorize(Roles = "administrador")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Administradores()
        {
            var administradores = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 2).OrderByDescending(u => u.UsuarioId).ToList();
            return View(administradores);
        }
        public IActionResult Coachs()
        {
            var coachs = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 5).OrderByDescending(u => u.UsuarioId).ToList();
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


        [HttpGet]
        public IActionResult AgregarUsuario(string url)
        {
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

        public IActionResult SubirImagenes(string? archivoError)
        {
            if (archivoError != null)
            {
                ViewBag.Error = archivoError;
            }
            return View();
        }

        [HttpPost]
        public IActionResult SubirImagenes(string? archivoError, int id, IFormFile file, string tipo)
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


                    return RedirectToAction("SubirImagenes", "Administrador", new { archivoError = respuesta }); //{ archivoError = "Archivo subido con éxito" });
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




        // Acción para mostrar la disponibilidad del árbitro
        public IActionResult Disponibilidad(int Id)
        {
            // Obtener la disponibilidad del árbitro
            List<DisponibilidadArbitro> disponibilidad = _generalServicio.ObtenerDisponibilidadArbitro(Id);
            Usuario usuario = _generalServicio.ObtenerUsuarioPorId(Id);
            ViewBag.UsuarioNombre = $"{usuario.Nombre}";
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }
            // Pasar la disponibilidad a la vista
            ViewBag.UsuarioNombre = $"{usuario.Nombre} {usuario.ApPaterno} {usuario.ApMaterno}";
            ViewBag.Usuario = usuario.UsuarioId;
            return View(disponibilidad);
        }


        //AgregarDispnibilidad
        [HttpPost]
        public IActionResult AgregarDisponibilidad(DisponibilidadArbitro disponibilidad)
        {
            if (ModelState.IsValid)
            {
                _generalServicio.AgregarDisponibilidadArbitro(disponibilidad);
                return RedirectToAction("Disponibilidad", new { id = disponibilidad.UsuarioId });
            }
            else
            {
                // Re-obtener la lista de disponibilidades para el usuario actual
                var disponibilidades = _generalServicio.ObtenerDisponibilidadArbitro(disponibilidad.UsuarioId);
                var usuario = _generalServicio.ObtenerUsuarioPorId(disponibilidad.UsuarioId);

                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                ViewBag.UsuarioNombre = $"{usuario.Nombre} {usuario.ApPaterno} {usuario.ApMaterno}";
                ViewBag.Usuario = disponibilidad.UsuarioId;
                return View("Disponibilidad", disponibilidades);
            }
        }


        //Equipos
        public IActionResult Benjamines(string? archivoError)
        {
            if (archivoError != null)
            {
                ViewBag.Error = archivoError;
            }
            var EquiposBenjamines = _generalServicio.ObtenerEquipos().Where(e => e.Categoria == "Benjamines").OrderByDescending(e => e.EquipoId).ToList();
            return View(EquiposBenjamines);
        }

        public IActionResult Infantiles(string? archivoError)
        {
            if (archivoError != null)
            {
                ViewBag.Error = archivoError;
            }
            var equipos = _generalServicio.ObtenerEquipos().Where(e => e.Categoria == "Infantiles").OrderByDescending(e => e.EquipoId).ToList();
            return View(equipos);
        }

        public IActionResult Juveniles(string? archivoError)
        {
            var equipos = _generalServicio.ObtenerEquipos().Where(e => e.Categoria == "Juveniles").ToList();
            return View(equipos);
        }


        [HttpGet]


        [HttpPost]
        public IActionResult AgregarEquipo(Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int equipoId = _generalServicio.AgregarEquipo(equipo.EquipoNombre, equipo.Genero, equipo.CategoriaId, equipo.UsuarioCoachId);
                    int CategoriaId = equipo.CategoriaId;
                    return RedirectToAction("SubirLogo", new { equipoId, equipo.CategoriaId });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var categorias = _generalServicio.ObtenerCategorias();
            var coaches = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 5).OrderByDescending(u => u.UsuarioId).ToList();

            ViewBag.Categorias = new SelectList(categorias, "CategoriaId", "CategoriaNombre");
            ViewBag.Coaches = new SelectList(coaches, "UsuarioId", "NombreCompleto");

            return View(equipo);
        }
        [HttpPost]
        public IActionResult SubirLogo(string? archivoError, int id, IFormFile file, string tipo, int equipoId, int CategoriaId)
        {
            if (file != null && file.Length > 0)
            {
                if (!string.IsNullOrEmpty(tipo))
                {
                    string nombre = tipo + "_" + id + "_" + file.FileName.Replace(" ", "");
                    string respuesta = _archivosServicio.SubirArchivo(file, nombre, tipo);

                    if (respuesta == "Archivo subido con éxito.")
                    {
                        _archivosServicio.GuardarArchivoFotoEnBD(nombre, id, tipo);

                        switch (CategoriaId)
                        {
                            case 1:
                                return RedirectToAction("Benjamines", "Administrador", new { archivoError = respuesta });
                            case 2:
                                return RedirectToAction("Infantiles", "Administrador", new { archivoError = respuesta });
                            case 3:
                                return RedirectToAction("Juveniles", "Administrador", new { archivoError = respuesta });
                            default:
                                return RedirectToAction("SubirLogo", "Administrador", new { archivoError = "Categoría no válida.", equipoId, CategoriaId });
                        }
                    }
                    else
                    {
                        return RedirectToAction("SubirLogo", "Administrador", new { archivoError = respuesta, equipoId, CategoriaId });
                    }
                }
                else
                {
                    return RedirectToAction("SubirLogo", "Administrador", new { archivoError = "Por favor, selecciona un tipo de archivo válido.", equipoId, CategoriaId });
                }
            }
            else
            {
                return RedirectToAction("SubirLogo", "Administrador", new { archivoError = "Por favor, selecciona un archivo válido.", equipoId, CategoriaId });
            }
        }


        [HttpGet]
        public IActionResult SubirLogo(string? archivoError, int equipoId, int CategoriaId)
        {
            if (archivoError != null)
            {
                ViewBag.Error = archivoError;
            }
            ViewBag.equipoId = equipoId;
            ViewBag.categorias = CategoriaId;
            return View();
        }


        [HttpGet]
        public IActionResult DetallesEquipo(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID de equipo inválido.");
            }
            var equipo = _generalServicio.ObtenerEquipoPorId(id);
            var jugadores = _generalServicio.ObtenerJugadoresPorEquipoId(id);
            var categoria = _generalServicio.ObtenerCategoriaPorId(equipo.CategoriaId);

            var coach = _generalServicio.ObtenerUsuarioPorId(equipo.UsuarioCoachId);
            var torneo = _generalServicio.ObtenerTorneos;

            var viewModel = new DetallesEquipoViewModel
            {
                Equipo = equipo,
                Jugadores = jugadores,
                Categoria = categoria,
                Coach = coach
            };


            return View(viewModel);
        }



        public IActionResult AgregarJugadores(int equipoId)
        {
            ViewBag.EquipoId = equipoId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarJugadores(Jugador jugador)
        {
            if (ModelState.IsValid)
            {
                int jugadorId = _generalServicio.InsertarJugador(jugador);
                return RedirectToAction("Fotografia", "Administrador", new { id = jugador.EquipoId, jugadorId = jugadorId });
            }
            return View(jugador);
        }

        public IActionResult Fotografia(int id, int jugadorId)
        {
            ViewBag.IdEquipo = id;

            ViewBag.Id = jugadorId;
            return View();
        }
        [HttpPost]
        public IActionResult Fotografia(string? archivoError, int id, IFormFile file, string tipo, int EquipoId)
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


                    return RedirectToAction("DetallesEquipo", "Administrador", new { id = EquipoId, archivoError = respuesta });
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