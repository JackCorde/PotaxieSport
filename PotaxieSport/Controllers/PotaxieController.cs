using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using PotaxieSport.Models.ViewModels;
using System.Data;

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

        [HttpPost]
        [Route("LoginDoctor")]
        public IActionResult LoginDoctor([FromBody] LoginDoctor data)
        {
            DatosDoctor datosDoctor = new DatosDoctor();
            try
            {
                Usuario currentUser = _generalServicio.ConseguirUsuario(data.correo);
                if (currentUser != null)
                {
                    bool matchPassword = BCrypt.Net.BCrypt.Verify(data.password, currentUser.Password);
                    
                    if (matchPassword)
                    {
                        if(currentUser.RolId != 1)
                        {
                            return StatusCode(StatusCodes.Status200OK, new { mensaje = "No eres un usuario de tipo 'Doctor'" });
                        }
                        var listaTorneos = ObtenerTorneosDoctor(currentUser.UsuarioId);
                        if (listaTorneos.Count > 0)
                        {
                            datosDoctor = new DatosDoctor
                            {
                                doctor = currentUser,
                                torneos = listaTorneos
                            };
                            return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = datosDoctor });
                        }
                        return StatusCode(StatusCodes.Status200OK, new { mensaje = "El doctor no tiene torneos asignados"});
                    }
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Contraseña Incorrecta" });
                    
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Usuario no encontrado" });
               
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }


        private List<Torneos> ObtenerTorneosDoctor(int doctorId)
        {
            try {
                List<Torneos> listaTorneos = new();

                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    // Cambia el comando para llamar a la función
                    using (var cmd = new NpgsqlCommand("SELECT * FROM ObtenerTorneosDoctor(@doctorId)", connection))
                    {
                        cmd.CommandType = CommandType.Text; // Cambiar a CommandType.Text
                        cmd.Parameters.AddWithValue("@doctorId", doctorId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var equiposParticipantes = EquiposPorTorneo(reader.GetInt32(reader.GetOrdinal("torneo_id")));
                                Torneo torneo = new Torneo
                                {
                                    TorneoId = reader.GetInt32(reader.GetOrdinal("torneo_id")),
                                    NombreTorneo = reader.IsDBNull(reader.GetOrdinal("nombre_torneo")) ? null : reader.GetString(reader.GetOrdinal("nombre_torneo")),
                                    CategoriaId = reader.GetInt32(reader.GetOrdinal("categoria_id")),
                                    Categoria = reader.IsDBNull(reader.GetOrdinal("categoria")) ? null : reader.GetString(reader.GetOrdinal("categoria")),
                                    Genero = reader.IsDBNull(reader.GetOrdinal("genero")) ? null : reader.GetString(reader.GetOrdinal("genero")),
                                    Logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString(reader.GetOrdinal("logo")),
                                    AdministradorId = reader.GetInt32(reader.GetOrdinal("usuario_admin")),
                                    Administrador = reader.IsDBNull(reader.GetOrdinal("administrador")) ? null : reader.GetString(reader.GetOrdinal("administrador")),
                                    ContadorId = reader.GetInt32(reader.GetOrdinal("usuario_contador")),
                                    Contador = reader.IsDBNull(reader.GetOrdinal("contador")) ? null : reader.GetString(reader.GetOrdinal("contador")),
                                    DoctorId = reader.GetInt32(reader.GetOrdinal("usuario_doctor")),
                                    Doctor = reader.IsDBNull(reader.GetOrdinal("doctor")) ? null : reader.GetString(reader.GetOrdinal("doctor")),
                                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
                                    FechaFin = reader.GetDateTime(reader.GetOrdinal("fecha_fin"))
                                };

                                Torneos elemento = new Torneos()
                                {
                                    torneo = torneo,
                                    equipos = equiposParticipantes
                                };
                                
                                listaTorneos.Add(elemento);
                            }
                        }
                    }
                }
                return listaTorneos;
            }
            catch(Exception ex)
            {
                string mensaje = ex.Message;
                return null;
            }
        }

        private List<Equipos> EquiposPorTorneo(int torneoId)
        {
            try
            {
                List<Equipos> listaEquipos = new();
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    // Cambia el comando para llamar a la función
                    using (var cmd = new NpgsqlCommand("SELECT * FROM EquiposDeTorneo(@p_torneo_id)", connection))
                    {
                        cmd.CommandType = CommandType.Text; // Cambiar a CommandType.Text
                        cmd.Parameters.AddWithValue("@p_torneo_id", torneoId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var jugadoresEquipo = JugadoresPorEquipo(reader.GetInt32(reader.GetOrdinal("equipo_id")));
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

                                Equipos elemento = new Equipos
                                {
                                    equipo = equipo,
                                    jugadores = jugadoresEquipo
                                };

                                listaEquipos.Add(elemento);
                            }
                        }
                    }
                }

                return listaEquipos;

            } 
            catch (Exception ex) {
                string mensaje = ex.Message;
                return null;
            }
        }


        private List<Jugadores> JugadoresPorEquipo(int equipoId) 
        {
            try { 
                List<Jugadores> listaJugadores = new();
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    // Cambia el comando para llamar a la función con el parámetro equipoId
                    using (var cmd = new NpgsqlCommand("SELECT * FROM ObtenerJugadoresPorEquipo(@p_equipo_id)", connection))
                    {
                        cmd.CommandType = CommandType.Text; // Cambiar a CommandType.Text
                        cmd.Parameters.AddWithValue("@p_equipo_id", equipoId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var listaRegistroSalud = ObtenerDatosSalud(reader.GetInt32(reader.GetOrdinal("jugador_id")));
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

                                Jugadores elemento = new Jugadores()
                                {
                                    jugador=jugador,
                                    registros = listaRegistroSalud
                                };

                                listaJugadores.Add(elemento);
                            }
                        }
                    }
                }

                return listaJugadores;
            }
            catch (Exception ex) {
                string mensaje = ex.Message;
                return null;
            }
        }



        //   ***        API PARA JUGADORES         ***    

        [HttpPost]
        [Route("LoginJugador")]
        public IActionResult LoginJugador([FromBody] Login data)
        {
            DatosJugador datosJugador = new DatosJugador();
            try
            {
                var infoJugador = ObtenerJugadorDatos(data.username);
                if(infoJugador != null)
                {
                    if(infoJugador.Clave == data.password)
                    {
                        var registrosSalud = ObtenerDatosSalud(infoJugador.JugadorId);
                        var torneoActual = ObtenerTorneo(infoJugador.EquipoId);
                        datosJugador = new DatosJugador
                        {
                            jugador = infoJugador,
                            registros = registrosSalud,
                            torneo = torneoActual
                        };

                        return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = datosJugador });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status200OK, new { mensaje = "Contraseña Incorrecta" });
                    }
                    
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Usuario no encontrado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        

        private Jugador ObtenerJugadorDatos(string username)
        {
            Jugador jugador = null;
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM loginJugador(@j_username)", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@j_username", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int edad = reader.GetInt32(reader.GetOrdinal("edad"));
                            var resultados = CalcularFrecuenciaCardiaca(edad);

                            jugador = new Jugador()
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
                                NumJugador = reader.GetInt32(reader.GetOrdinal("num_jugador")),
                                Clave = reader.IsDBNull(reader.GetOrdinal("contrasena"))? null : reader.GetString(reader.GetOrdinal("contrasena")),
                                FrecuenciaMaxima = resultados.FCM,
                                FrecuenciaMinima = resultados.FCMin
                            };
                            return jugador;
                        }

                        return jugador;
                    }
                }
            }
        }

        

        public Torneo ObtenerTorneo(int equipoId)
        {
            Torneo torneo = null;
            try
            {
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    // Cambia el comando para llamar a la función
                    using (var cmd = new NpgsqlCommand("SELECT * FROM ObtenerTorneoActivo(@equipoId)", connection))
                    {
                        cmd.CommandType = CommandType.Text; // Cambiar a CommandType.Text
                        cmd.Parameters.AddWithValue("@equipoId", equipoId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var respuesta = ConsultarPartido(reader.GetInt32(reader.GetOrdinal("torneo_id")), equipoId);
                                torneo = new Torneo
                                {
                                    TorneoId = reader.GetInt32(reader.GetOrdinal("torneo_id")),
                                    NombreTorneo = reader.IsDBNull(reader.GetOrdinal("nombre_torneo")) ? null : reader.GetString(reader.GetOrdinal("nombre_torneo")),
                                    CategoriaId = reader.GetInt32(reader.GetOrdinal("categoria_id")),
                                    Categoria = reader.IsDBNull(reader.GetOrdinal("categoria")) ? null : reader.GetString(reader.GetOrdinal("categoria")),
                                    Genero = reader.IsDBNull(reader.GetOrdinal("genero")) ? null : reader.GetString(reader.GetOrdinal("genero")),
                                    Logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString(reader.GetOrdinal("logo")),
                                    AdministradorId = reader.GetInt32(reader.GetOrdinal("usuario_admin")),
                                    Administrador = reader.IsDBNull(reader.GetOrdinal("administrador")) ? null : reader.GetString(reader.GetOrdinal("administrador")),
                                    ContadorId = reader.GetInt32(reader.GetOrdinal("usuario_contador")),
                                    Contador = reader.IsDBNull(reader.GetOrdinal("contador")) ? null : reader.GetString(reader.GetOrdinal("contador")),
                                    DoctorId = reader.GetInt32(reader.GetOrdinal("usuario_doctor")),
                                    Doctor = reader.IsDBNull(reader.GetOrdinal("doctor")) ? null : reader.GetString(reader.GetOrdinal("doctor")),
                                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
                                    FechaFin = reader.GetDateTime(reader.GetOrdinal("fecha_fin")),
                                    EnPartido = respuesta
                                };

                                return torneo;
                            }
                        }
                    }
                }
                return torneo;
            }
            catch(Exception ex)
            {
                string mensaje = ex.Message;
                return torneo;
            }
        }

        private bool ConsultarPartido(int torneoId, int equipoId)
        {
            bool enJuego = false;
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM consultarPartido(@torneoId, @equipoId)", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@torneoId", torneoId);
                    cmd.Parameters.AddWithValue("@equipoId", equipoId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            enJuego = reader.GetBoolean(reader.GetOrdinal("consultarPartido"));
                        }
                    }
                }
            }
            return enJuego;
        }

        private List<RegistroSalud> ObtenerDatosSalud(int id)
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

                return registrosSalud;
            }
            catch
            {
                return registrosSalud;
            }
        }

        private static (int FCM, int FCMin) CalcularFrecuenciaCardiaca(int edad)
        {
            int FCM = 220 - edad;
            int FCMin = CalcularFCMin(); // Aquí asumimos que se mide directamente. Cambiar según necesidad.

            return (FCM, FCMin);
        }

        // Método para obtener la frecuencia cardíaca en reposo. Aquí se puede reemplazar por una medición real.
        private static int CalcularFCMin()
        {
            // Asumimos un valor típico de frecuencia cardíaca en reposo. Cambiar según necesidad.
            return 60;
        }


    }
}
