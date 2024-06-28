using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using PotaxieSport.Models.ViewModels;
using System.Data;
using System.Text.Json;

namespace PotaxieSport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PotaxieController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly GeneralServicio _generalServicio;
        public PotaxieController(Contexto contexto)
        {
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
        }

        [HttpGet]
        [Route("Torneos")]
        public IActionResult Torneos()
        {
            List<Torneo> torneos = new List<Torneo>();
            try
            {
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    // Cambia el comando para llamar a la función
                    using (var cmd = new NpgsqlCommand("SELECT * FROM ObtenerTorneos()", connection))
                    {
                        cmd.CommandType = CommandType.Text; // Cambiar a CommandType.Text

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var torneo = new Torneo
                                {
                                    TorneoId = reader.GetInt32(reader.GetOrdinal("torneo_id")),
                                    NombreTorneo = reader.IsDBNull(reader.GetOrdinal("nombre_torneo")) ? null : reader.GetString(reader.GetOrdinal("nombre_torneo")),
                                    CategoriaId = reader.GetInt32(reader.GetOrdinal("categoria_id")),
                                    Categoria = reader.IsDBNull(reader.GetOrdinal("categoria")) ? null : reader.GetString(reader.GetOrdinal("categoria")),
                                    Genero = reader.IsDBNull(reader.GetOrdinal("genero")) ? null : reader.GetString(reader.GetOrdinal("genero")),
                                    Logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString(reader.GetOrdinal("logo")),
                                    AdministradorId = reader.GetInt32(reader.GetOrdinal("administrador_id")),
                                    Administrador = reader.IsDBNull(reader.GetOrdinal("administrador")) ? null : reader.GetString(reader.GetOrdinal("administrador")),
                                    ContadorId = reader.GetInt32(reader.GetOrdinal("contador_id")),
                                    Contador = reader.IsDBNull(reader.GetOrdinal("contador")) ? null : reader.GetString(reader.GetOrdinal("contador")),
                                    DoctorId = reader.GetInt32(reader.GetOrdinal("doctor_id")),
                                    Doctor = reader.IsDBNull(reader.GetOrdinal("doctor")) ? null : reader.GetString(reader.GetOrdinal("doctor")),
                                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
                                    FechaFin = reader.GetDateTime(reader.GetOrdinal("fecha_fin"))
                                };

                                torneos.Add(torneo);
                            }
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new {mensaje="ok", response = torneos});
            }
            catch(Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = torneos });
            }
        }


        [HttpGet]
        [Route("Equipos")]
        public IActionResult Equipos()
        {
            List<Equipo> equipos = new List<Equipo>();
            try
            {
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    // Cambia el comando para llamar a la función
                    using (var cmd = new NpgsqlCommand("SELECT * FROM ObtenerEquipos()", connection))
                    {
                        cmd.CommandType = CommandType.Text; // Cambiar a CommandType.Text

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var equipo = new Equipo
                                {
                                    EquipoId = reader.GetInt32(reader.GetOrdinal("equipo_id")),
                                    EquipoNombre = reader.IsDBNull(reader.GetOrdinal("nombre_equipo")) ? null : reader.GetString(reader.GetOrdinal("nombre_equipo")),
                                    Genero = reader.IsDBNull(reader.GetOrdinal("genero")) ? null : reader.GetString(reader.GetOrdinal("genero")),
                                    Logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString(reader.GetOrdinal("logo")),
                                    CategoriaId = reader.GetInt32(reader.GetOrdinal("categoria_id")),
                                    Categoria = reader.IsDBNull(reader.GetOrdinal("categoria_nombre")) ? null : reader.GetString(reader.GetOrdinal("categoria_nombre")),
                                    UsuarioCoachId = reader.GetInt32(reader.GetOrdinal("usuario_coach")),
                                    Coach = reader.IsDBNull(reader.GetOrdinal("nombre_coach")) ? null : reader.GetString(reader.GetOrdinal("nombre_coach"))
                                };

                                equipos.Add(equipo);
                            }
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = equipos });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = equipos });
            }
        }

        [HttpPost]
        [Route("Jugadores")]
        public IActionResult Jugadores([FromBody] Request request)
        {
            List<Jugador> jugadores = new List<Jugador>();
            try
            {
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    // Cambia el comando para llamar a la función con el parámetro equipoId
                    using (var cmd = new NpgsqlCommand("SELECT * FROM ObtenerJugadoresPorEquipo(@equipo_id)", connection))
                    {
                        cmd.CommandType = CommandType.Text; // Cambiar a CommandType.Text
                        cmd.Parameters.AddWithValue("equipo_id", request.EquipoId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var jugador = new Jugador
                                {
                                    JugadorId = reader.GetInt32(reader.GetOrdinal("jugador_id")),
                                    JugadorNombre = reader.IsDBNull(reader.GetOrdinal("jugador_nombre")) ? null : reader.GetString(reader.GetOrdinal("jugador_nombre")),
                                    ApPaterno = reader.IsDBNull(reader.GetOrdinal("ap_paterno")) ? null : reader.GetString(reader.GetOrdinal("ap_paterno")),
                                    ApMaterno = reader.IsDBNull(reader.GetOrdinal("ap_materno")) ? null : reader.GetString(reader.GetOrdinal("ap_materno")),
                                    Edad = reader.GetInt32(reader.GetOrdinal("edad")),
                                    Fotografia = reader.IsDBNull(reader.GetOrdinal("fotografia")) ? null : reader.GetString(reader.GetOrdinal("fotografia")),
                                    EquipoId = reader.GetInt32(reader.GetOrdinal("equipo_id")),
                                    Equipo = reader.IsDBNull(reader.GetOrdinal("equipo")) ? null : reader.GetString(reader.GetOrdinal("equipo")),
                                    Posicion = reader.IsDBNull(reader.GetOrdinal("posicion")) ? null : reader.GetString(reader.GetOrdinal("posicion")),
                                    NumJugador = reader.GetInt32(reader.GetOrdinal("num_jugador"))
                                };

                                jugadores.Add(jugador);
                            }
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = jugadores });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = jugadores });
            }
        }


        [HttpPost]
        [Route("Salud")]
        public IActionResult Salud([FromBody] Request request)
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
                        cmd.Parameters.AddWithValue("p_jugador_id", request.JugadorId);

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

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = registrosSalud });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = registrosSalud });
            }
        }


        [HttpPost]
        [Route("CrearRegistroSalud")]
        public IActionResult CrearRegistroSalud([FromBody] Request request)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM CrearRegistroSalud(@p_jugador_id, @p_frecuencia_card)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_jugador_id", request.JugadorId);
                        cmd.Parameters.AddWithValue("p_frecuencia_card", request.FrecuenciaCard);

                        cmd.ExecuteNonQuery();
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Registro de salud creado exitosamente" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }


    }
}
