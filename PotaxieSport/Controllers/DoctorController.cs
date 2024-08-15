using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using System.Data;
using System.Diagnostics;

namespace PotaxieSport.Controllers
{
    public class DoctorController : Controller
    {
        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        private readonly TorneoServicio _torneoServicio;

        public DoctorController(Contexto contexto)
        {
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
            _torneoServicio = new TorneoServicio(contexto);
        }

        [Authorize]
        public IActionResult Index(int equipoId, int torneoId)
        {
            var jugadores = _torneoServicio.JugadoresPorEquipo(equipoId);
            ViewBag.TorneoId = torneoId;
            ViewBag.Jugadores = jugadores;
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


        public IActionResult ObtenerDatosSalud(int id)
        {
            List<RegistroSalud> registrosSalud = new List<RegistroSalud>();

            try
            {

                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("Select * from ObtenerRegistrosSalud(@p_jugador_id)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_jugador_id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var registro = new RegistroSalud
                                {
                                    RegistroSaludId = reader.GetInt32(reader.GetOrdinal("registro_salud_id")),
                                    JugadorId = reader.GetInt32(reader.GetOrdinal("jugador_id")),
                                    Jugador = reader.IsDBNull(reader.GetOrdinal("jugador")) ? null : reader.GetString(reader.GetOrdinal("jugador")),
                                    FrecuenciaCardiaca = reader.GetInt32(reader.GetOrdinal("frecuencia_card")),
                                    Estatus = reader.IsDBNull(reader.GetOrdinal("estatus")) ? null : reader.GetString(reader.GetOrdinal("estatus")),
                                    Fecha = reader.GetDateTime(reader.GetOrdinal("fecha"))
                                };

                                registrosSalud.Add(registro);
                            }
                        }
                    }
                }

                return Json(registrosSalud);
            }
            catch
            {
                return Json(registrosSalud);
            }
        }
    }
}
