using Microsoft.AspNetCore.Authorization;
using Npgsql;
using PotaxieSport.Models;
using PotaxieSport.Models.ViewModels;
using System.Data;

namespace PotaxieSport.Data.Servicios
{
    public class TorneoServicio
    {
        private readonly Contexto _contexto;

        public TorneoServicio(Contexto contexto)
        {
            _contexto = contexto;
        }

        [Authorize]
        public DetallesTorneo ObtenerTorneo(int torneoId)
        {
            DetallesTorneo torneoDetalles = new();
            try
            {
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    // Cambia el comando para llamar a la función
                    using (var cmd = new NpgsqlCommand("SELECT * FROM ObtenerTorneoPorId(@torneoId)", connection))
                    {
                        cmd.CommandType = CommandType.Text; // Cambiar a CommandType.Text
                        cmd.Parameters.AddWithValue("@torneoId", torneoId);

                        using (var reader = cmd.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                Torneo torneoObtenido = new Torneo
                                {
                                    TorneoId = reader.GetInt32(reader.GetOrdinal("torneo_id")),
                                    NombreTorneo = reader.IsDBNull(reader.GetOrdinal("nombre_torneo")) ? null : reader.GetString(reader.GetOrdinal("nombre_torneo")),
                                    CategoriaId = reader.GetInt32(reader.GetOrdinal("categoria_id")),
                                    Categoria = reader.IsDBNull(reader.GetOrdinal("categoria")) ? null : reader.GetString(reader.GetOrdinal("categoria")),
                                    Genero = reader.IsDBNull(reader.GetOrdinal("genero")) ? null : reader.GetString(reader.GetOrdinal("genero")),
                                    Logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : "~/Formatos/Imagenes/Torneo/" + reader.GetString(reader.GetOrdinal("logo")),
                                    AdministradorId = reader.GetInt32(reader.GetOrdinal("usuario_admin")),
                                    Administrador = reader.IsDBNull(reader.GetOrdinal("administrador")) ? null : reader.GetString(reader.GetOrdinal("administrador")),
                                    ContadorId = reader.GetInt32(reader.GetOrdinal("usuario_contador")),
                                    Contador = reader.IsDBNull(reader.GetOrdinal("contador")) ? null : reader.GetString(reader.GetOrdinal("contador")),
                                    DoctorId = reader.GetInt32(reader.GetOrdinal("usuario_doctor")),
                                    Doctor = reader.IsDBNull(reader.GetOrdinal("doctor")) ? null : reader.GetString(reader.GetOrdinal("doctor")),
                                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
                                    FechaFin = reader.GetDateTime(reader.GetOrdinal("fecha_fin"))
                                };

                                List<Equipos> equipos = EquiposPorTorneo(torneoId);
                                List<Partido> partidos = ObtenerPartidos().Where(p => p.TorneoId == torneoId).ToList();

                                torneoDetalles = new DetallesTorneo
                                {
                                    torneo = torneoObtenido,
                                    equipos = equipos,
                                    partidos = partidos
                                };
                            }
                            else
                            {
                                // Manejo del caso cuando no se encuentra el torneo
                                throw new Exception("No se encontró ningún torneo con el ID especificado.");
                            }

                        }
                    }
                }

                return torneoDetalles;
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return torneoDetalles;
            }
        }

        private List<Partido> ObtenerPartidos()
        {
            List<Partido> partidos = new List<Partido>();
            try
            {
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    // Cambia el comando para llamar a la función
                    using (var cmd = new NpgsqlCommand("SELECT * FROM ObtenerPartidos()", connection))
                    {
                        cmd.CommandType = CommandType.Text; // Cambiar a CommandType.Text

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var partido = new Partido
                                {
                                    PartidoId = reader.GetInt32(reader.GetOrdinal("partido_id")),
                                    TorneoId = reader.GetInt32(reader.GetOrdinal("torneo_id")),
                                    Torneo = reader.IsDBNull(reader.GetOrdinal("torneo")) ? null : reader.GetString(reader.GetOrdinal("torneo")),
                                    EquipoRetadorId = reader.GetInt32(reader.GetOrdinal("equipo_retador")),
                                    EquipoRetador = reader.IsDBNull(reader.GetOrdinal("retador")) ? null : reader.GetString(reader.GetOrdinal("retador")),
                                    EquipoDefensorId = reader.GetInt32(reader.GetOrdinal("equipo_defensor")),
                                    EquipoDefensor = reader.IsDBNull(reader.GetOrdinal("defensor")) ? null : reader.GetString(reader.GetOrdinal("defensor")),
                                    EquipoGanadorId = reader.GetInt32(reader.GetOrdinal("equipo_ganador")),
                                    EquipoGanador = reader.IsDBNull(reader.GetOrdinal("ganador")) ? "Aun no hay Ganador" : reader.GetString(reader.GetOrdinal("ganador")),
                                    UsuarioArbitro = reader.GetInt32(reader.GetOrdinal("usuario_arbitro")),
                                    Arbitro = reader.IsDBNull(reader.GetOrdinal("arbitro")) ? null : reader.GetString(reader.GetOrdinal("arbitro")),
                                    Cedula = reader.IsDBNull(reader.GetOrdinal("cedula")) ? "Aun no hay cedula" : reader.GetString(reader.GetOrdinal("cedula")),
                                    Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                    Hora = reader.GetTimeSpan(reader.GetOrdinal("hora")),
                                    Lugar = reader.IsDBNull(reader.GetOrdinal("lugar")) ? null : reader.GetString(reader.GetOrdinal("lugar")),
                                    Costo = reader.GetDecimal(reader.GetOrdinal("costo")),
                                };

                                partidos.Add(partido);
                            }
                        }
                    }
                }

                return partidos;
            }
            catch (Exception error)
            {
                string mensaje = error.Message;
                return partidos;
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
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return null;
            }
        }


        private List<Jugadores> JugadoresPorEquipo(int equipoId)
        {
            try
            {
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
                                    jugador = jugador,
                                    registros = listaRegistroSalud
                                };

                                listaJugadores.Add(elemento);
                            }
                        }
                    }
                }

                return listaJugadores;
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return null;
            }
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


        private List<MovimientoEconomico> ObtenerMovimientosPorTorneo(int torneoId)
        {
            List<MovimientoEconomico> movimientos = new();

            return movimientos;
        }


    }
}
