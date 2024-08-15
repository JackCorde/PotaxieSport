using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Npgsql;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using System.Data;
using System.Diagnostics;

namespace PotaxieSport.Controllers
{
    public class ArbitroController : Controller
    {
        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        private readonly ArchivosServicio _archivosServicio;
        private readonly ILogger<HomeController> _logger;

        public ArbitroController(ILogger<HomeController> logger, Contexto contexto, IWebHostEnvironment hostingEnvironment)
        {
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
            _logger = logger;
            _archivosServicio = new ArchivosServicio(contexto, hostingEnvironment);
        }
        [Authorize(Roles = "arbitro")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AgregarCedula(int partido, IFormFile archivoCedula, int torneo)
        {
            if (archivoCedula != null && archivoCedula.Length > 0)
            {
                //Asigna nombre al archivo con la estructura tipo_id_nombre.dominio  (El remplace quita los espacios)
                string nombre = archivoCedula.FileName.Replace(" ", "");

                //Llamar a la función que sube el archivo a las carpetas de ASP.NET
                string respuesta = _archivosServicio.SubirArchivo(archivoCedula, nombre, "Cedula");


                // Aquí puedes llamar a un procedimiento almacenado para registrar la información en la base de datos
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM RegistrarCedula(@partidoId, @rutaArchivo)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@partidoId", partido);
                        cmd.Parameters.AddWithValue("@rutaArchivo", nombre);

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Informacion", "Compartido", new { torneoId = torneo });
            }

            // Si no se sube ningún archivo, puedes manejar el error aquí
            return RedirectToAction("Informacion", "Compartido", new { torneoId = torneo });
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
