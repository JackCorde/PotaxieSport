using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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
    public class HomeController : Controller
    {

        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Contexto contexto)
        {

            _logger = logger;
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
        }

        public IActionResult Index()
        {
            ClaimsPrincipal c = HttpContext.User;

            if (c.Identity != null && c.Identity.IsAuthenticated)
            {
                // Verificar si el usuario tiene el claim de rol
                var roleClaim = c.FindFirst(ClaimTypes.Role);

                if (roleClaim != null)
                {
                    // Obtener el valor del claim de rol
                    string role = roleClaim.Value;

                    // Redirigir al index del controlador adecuado según el rol
                    switch (role)
                    {
                        case "administrador":
                            return RedirectToAction("Index", "Administrador");
                        case "doctor":
                            return RedirectToAction("Index", "Doctor");
                        case "arbitro":
                            return RedirectToAction("Index", "Arbitro");
                        case "contador":
                            return RedirectToAction("Index", "Contador");
                        case "coach":
                            return RedirectToAction("Index", "Coach");
                        default:
                            return RedirectToAction("Index", "Home");
                    }
                }
            }
            var torneos = _generalServicio.ObtenerTorneos();
            ViewBag.Torneos = torneos;
            return View();
        }

        public IActionResult Torneos()
        {
            var torneos = _generalServicio.ObtenerTorneos();
            ViewBag.Torneos = torneos;
            return View();
        }

        public IActionResult Equipos()
        {
            var equipos = _generalServicio.ObtenerEquipos();
            ViewBag.Equipos = equipos;
            return View();
        }

        public IActionResult Partidos()
        {
            var partidos = _generalServicio.ObtenerPartidos();
            ViewBag.Partidos = partidos;
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLogin model)
        {
            try
            {
                using (NpgsqlConnection con = new(_contexto.Conexion))
                {
                    using (NpgsqlCommand cmd = new("SELECT * FROM ValidarUsuario(@p_correo)", con))
                    {
                        cmd.CommandType = CommandType.Text;

                        #pragma warning disable CS8604 // Posible argumento de referencia nulo
                        cmd.Parameters.AddWithValue("p_correo", model.Correo);
                        con.Open();
                        try
                        {
                            using (var dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    int numeroIntentos = (int)dr["error_autentificacion"];
                                    if (numeroIntentos <= 3)
                                    {
                                        bool passwordMatch = BCrypt.Net.BCrypt.Verify(model.LaPoderosa, dr["password"].ToString());
                                        if (passwordMatch)
                                        {

                                            int usuarioId = (int)dr["usuario_id"];
                                            _generalServicio.LimpiarNumeroIntento(usuarioId);
                                            string? nombreusuario = (string)dr["username"];
                                            int idUsuario = (int)dr["usuario_id"];
                                            string? nombreCompleto = (string)dr["nombre"] + " " + (string)dr["ap_paterno"] + " " + (string)dr["ap_materno"];

                                            if (nombreusuario != null)
                                            {
                                                var claims = new List<Claim>()
                                            {
                                                new Claim(ClaimTypes.NameIdentifier, nombreusuario),
                                                new Claim(ClaimTypes.Name, nombreCompleto),
                                                new Claim(ClaimTypes.SerialNumber, idUsuario.ToString())
                                            };

                                                int perfilId = (int)dr["rol_id"];
                                                string perfilNombre = (string)dr["rol"];
                                                claims.Add(new Claim(ClaimTypes.Role, perfilNombre));

                                                var identify = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                                var propiedades = new AuthenticationProperties
                                                {
                                                    AllowRefresh = true,
                                                    IsPersistent = true,
                                                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(1)),
                                                };

                                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identify), propiedades);

                                                switch (perfilId)
                                                {
                                                    case 1:
                                                        return RedirectToAction("Torneos", "Compartido");
                                                    case 2:
                                                        return RedirectToAction("Torneos", "Compartido");
                                                    case 3:
                                                        return RedirectToAction("Torneos", "Compartido");
                                                    case 4:
                                                        return RedirectToAction("Index", "Arbitro");
                                                    case 5:
                                                        return RedirectToAction("Index", "Coach");
                                                }

                                            }

                                        }
                                        else
                                        {
                                            int usuarioId = (int)dr["usuario_id"];
                                            _generalServicio.NumeroIntento(usuarioId);
                                            ViewBag.Error = "Contraseña Incorrecta";
                                            dr.Close();
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.Error = "Cuenta Bloqueada por Exceso de Intentos";
                                        dr.Close();
                                    }

                                }
                                else
                                {
                                    ViewBag.Error = "Usuario no Registrado";
                                    dr.Close();
                                }
                            }
                        }
                        finally
                        {
                            if (cmd != null)
                                cmd.Dispose();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("Login", new { ViewBag.Error });
        }


        public IActionResult CambiarContraseña()
        {

            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM CambiarContraseña(@id, @lapoderosa)", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("id", 1);
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword("12345");
                    cmd.Parameters.AddWithValue("lapoderosa", hashedPassword);

                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }


        public ActionResult RegistrarAficionado(string correo)
        {
            Email email = new();
            email.RegistroAficionado(correo);
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
