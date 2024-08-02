using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using System.Data;


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

       
        [HttpGet]
        public IActionResult AgregarUsuario(string url)
        {
            // Asignar el par�metro url a ViewBag
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
                        ModelState.AddModelError("Email", "El correo electr�nico ya existe.");
                        return View("AgregarUsuario", model); // Retorna la vista con el mensaje de error
                    }
                    model.ErrorAutentificacion = 0;
                    _generalServicio.CrearUsuario(model.Nombre, model.ApPaterno, model.ApMaterno, model.Username, model.Email, model.RolId, model.ErrorAutentificacion, model.Password);
                    TempData["SuccessMessage"] = "Usuario registrado exitosamente.";
                    if (string.IsNullOrEmpty(url))
                    {
                        url = "Index";
                    }
                    return RedirectToAction(url); // Redirige a la acci�n especificada
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurri� un error al crear el usuario: " + ex.Message);
                    return View("AgregarUsuario", model); // Retorna la vista con el mensaje de error
                }
            }
            return View("AgregarUsuario", model);
        }

        // Acci�n para mostrar el formulario de actualizaci�n
        [HttpGet]
        public IActionResult ActualizarUsuario(int id)
        {
            // Obtener el usuario por ID
            var usuario = _generalServicio.ObtenerUsuarioPorId(id);

            if (usuario == null)
            {
                return NotFound(); // O redirigir a una p�gina de error
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
                    ModelState.AddModelError("", "Ocurri� un error al actualizar el usuario: " + ex.Message);
                }
            }

            // Si el modelo no es v�lido o hubo un error, regresa a la vista con el modelo
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

                    //Llamar a la funci�n que sube el archivo a las carpetas de ASP.NET
                    string respuesta = _archivosServicio.SubirArchivo(file, nombre, tipo);
                    
                    //Subir el archivo a la base de datos.
                    _archivosServicio.GuardarArchivoFotoEnBD(nombre, id, tipo);

                    
                    return RedirectToAction("SubirImagenes", "Administrador", new { archivoError = respuesta }); //{ archivoError = "Archivo subido con �xito" });
                }
                else
                {
                    return RedirectToAction("SubirImagenes", "Administrador", new { archivoError = "Por favor, selecciona un tipo de archivo v�lido." });
                }


            }
            else
            {
                return RedirectToAction("SubirImagenes", "Administrador", new { archivoError = "Por favor, selecciona un archivo v�lido." });
            }

        }




        // Acci�n para mostrar la disponibilidad del �rbitro
        public IActionResult Disponibilidad(int Id)
        {
            // Obtener la disponibilidad del �rbitro
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
        public IActionResult Benjamines()
        {
            var EquiposBenjamines = _generalServicio.ObtenerEquipos().Where(e => e.Categoria == "Benjamines").ToList();
            return View(EquiposBenjamines);
        }

        public IActionResult Infantiles()
        {
            var equipos = _generalServicio.ObtenerEquipos().Where(e => e.Categoria == "Infantiles").ToList();
            return View(equipos);
        }

        public IActionResult Juveniles()
        {
            var equipos = _generalServicio.ObtenerEquipos().Where(e => e.Categoria == "Juveniles").ToList();
            return View(equipos);
        }

        //AgregarEquipo
        public IActionResult AgregarEquipo()
        {
            var coachs = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 5)
               .OrderByDescending(u => u.UsuarioId)
               .ToList();
            ViewBag.Coaches = new SelectList(coachs, "UsuarioId", "NombreCompleto");
            return View();
        }


        [HttpPost]
        public IActionResult AgregarEquipo(Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                _generalServicio.AgregarEquipo(equipo);
                TempData["SuccessMessage"] = "Equipo agregado correctamente";

                // Redirige a la vista correspondiente seg�n la categor�a
                if (equipo.CategoriaId == 1) // ID de categor�a de Benjamines
                {
                    return RedirectToAction("Benjamines");
                }
                else if (equipo.CategoriaId == 2) // ID de categor�a de Infantiles
                {
                    return RedirectToAction("Infantiles");
                }
                else if (equipo.CategoriaId == 3) // ID de categor�a de Juveniles
                {
                    return RedirectToAction("Juveniles");
                }
            }

            // Si el modelo no es v�lido, recarga la lista de entrenadores para mostrarla nuevamente en la vista
            var coachs = _generalServicio.ObtenerUsuarios().Where(u => u.RolId == 5)
                .OrderByDescending(u => u.UsuarioId)
                .ToList();
            ViewBag.Coaches = new SelectList(coachs, "UsuarioId", "NombreCompleto");
            return View(equipo);
        }

    }
}
