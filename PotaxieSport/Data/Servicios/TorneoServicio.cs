using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

                                List<Equipo> equipos = ObtenerEquipos();
                                List<Equipo> equiposTorneo = equipos.Where(e => e.TorneoActualId == torneoId).ToList();
                                List<Equipo> equiposNoInscritos = equipos.Where(e => e.TorneoActualId == 0 && e.CategoriaId == reader.GetInt32(reader.GetOrdinal("categoria_id")) && e.Genero == reader.GetString(reader.GetOrdinal("genero"))).ToList();
                                List<MovimientoEconomico> movimientos = ObtenerMovimientos().Where(m => m.TorneoId == torneoId).ToList();
                                List<Partidos> partidos = ObtenerPartidos(torneoId);

                                torneoDetalles = new DetallesTorneo
                                {
                                    torneo = torneoObtenido,
                                    equipos = equiposTorneo,
                                    equiposNoInscritos = equiposNoInscritos,
                                    partidos = partidos,
                                    movimientos = movimientos
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

        private List<Partidos> ObtenerPartidos(int torneoId)
        {
            List<Partidos> partidos = new List<Partidos>();
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

                                if(reader.GetInt32(reader.GetOrdinal("torneo_id")) == torneoId)
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
                                        EquipoGanadorId = reader.IsDBNull(reader.GetOrdinal("ganador")) ? 0 : reader.GetInt32(reader.GetOrdinal("equipo_ganador")),
                                        EquipoGanador = reader.IsDBNull(reader.GetOrdinal("ganador")) ? "Aun no hay Ganador" : reader.GetString(reader.GetOrdinal("ganador")),
                                        UsuarioArbitro = reader.GetInt32(reader.GetOrdinal("usuario_arbitro")),
                                        Arbitro = reader.IsDBNull(reader.GetOrdinal("arbitro")) ? null : reader.GetString(reader.GetOrdinal("arbitro")),
                                        Cedula = reader.IsDBNull(reader.GetOrdinal("cedula")) ? null : reader.GetString(reader.GetOrdinal("cedula")),
                                        Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                        Hora = reader.GetTimeSpan(reader.GetOrdinal("hora")),
                                        Lugar = reader.IsDBNull(reader.GetOrdinal("lugar")) ? null : reader.GetString(reader.GetOrdinal("lugar")),
                                        Costo = reader.GetDecimal(reader.GetOrdinal("costo")),
                                    };

                                    var pagos = ObtenerPagos().Where(pp => pp.PartidoId == reader.GetInt32(reader.GetOrdinal("partido_id"))).ToList();
                                    var elemento = new Partidos
                                    {
                                        partido = partido,
                                        pagos = pagos
                                    };

                                    partidos.Add(elemento);
                                }
                                
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

        //private List<Equipos> EquiposPorTorneo(int torneoId)
        //{
        //    try
        //    {
        //        List<Equipos> listaEquipos = new();
        //        using (var connection = new NpgsqlConnection(_contexto.Conexion))
        //        {
        //            connection.Open();
        //            // Cambia el comando para llamar a la función
        //            using (var cmd = new NpgsqlCommand("SELECT * FROM EquiposDeTorneo(@p_torneo_id)", connection))
        //            {
        //                cmd.CommandType = CommandType.Text; // Cambiar a CommandType.Text
        //                cmd.Parameters.AddWithValue("@p_torneo_id", torneoId);
        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var jugadoresEquipo = JugadoresPorEquipo(reader.GetInt32(reader.GetOrdinal("equipo_id")));
        //                        var equipo = new Equipo
        //                        {
        //                            EquipoId = reader.GetInt32(reader.GetOrdinal("equipo_id")),
        //                            EquipoNombre = reader.IsDBNull(reader.GetOrdinal("nombre_equipo")) ? null : reader.GetString(reader.GetOrdinal("nombre_equipo")),
        //                            Genero = reader.IsDBNull(reader.GetOrdinal("genero")) ? null : reader.GetString(reader.GetOrdinal("genero")),
        //                            Logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString(reader.GetOrdinal("logo")),
        //                            CategoriaId = reader.GetInt32(reader.GetOrdinal("categoria_id")),
        //                            Categoria = reader.IsDBNull(reader.GetOrdinal("categoria_nombre")) ? null : reader.GetString(reader.GetOrdinal("categoria_nombre")),
        //                            UsuarioCoachId = reader.GetInt32(reader.GetOrdinal("usuario_coach")),
        //                            Coach = reader.IsDBNull(reader.GetOrdinal("nombre_coach")) ? null : reader.GetString(reader.GetOrdinal("nombre_coach"))
        //                        };

        //                        Equipos elemento = new Equipos
        //                        {
        //                            equipo = equipo,
        //                            jugadores = jugadoresEquipo
        //                        };

        //                        listaEquipos.Add(elemento);
        //                    }
        //                }
        //            }
        //        }

        //        return listaEquipos;

        //    }
        //    catch (Exception ex)
        //    {
        //        string mensaje = ex.Message;
        //        return null;
        //    }
        //}


        public List<Equipo> ObtenerEquipos()
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
                                    Logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : "/Formatos/Imagenes/Equipo/" + reader.GetString(reader.GetOrdinal("logo")),
                                    CategoriaId = reader.GetInt32(reader.GetOrdinal("categoria_id")),
                                    Categoria = reader.IsDBNull(reader.GetOrdinal("categoria_nombre")) ? null : reader.GetString(reader.GetOrdinal("categoria_nombre")),
                                    UsuarioCoachId = reader.GetInt32(reader.GetOrdinal("usuario_coach")),
                                    Coach = reader.IsDBNull(reader.GetOrdinal("nombre_coach")) ? null : reader.GetString(reader.GetOrdinal("nombre_coach")),
                                    TorneoActualId = reader.IsDBNull(reader.GetOrdinal("torneo_actual")) ? 0 : reader.GetInt32(reader.GetOrdinal("torneo_actual")),
                                    TorneoActual = reader.IsDBNull(reader.GetOrdinal("torneo")) ? "No hay torneo asignado" : reader.GetString(reader.GetOrdinal("torneo")),
                                };

                                equipos.Add(equipo);
                            }
                        }
                    }
                }

                return equipos;
            }
            catch (Exception error)
            {
                string mensaje = error.Message;
                return equipos;
            }
        }


        public List<Jugador> JugadoresPorEquipo(int equipoId)
        {
            try
            {
                List<Jugador> listaJugadores = new();
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

                               
                                listaJugadores.Add(jugador);
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

        public List<RegistroSalud> ObtenerDatosSalud(int id)
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


        private List<MovimientoEconomico> ObtenerMovimientos()
        {
            List<MovimientoEconomico> movimientos = new();
            try
            {

                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("Select * from obtener_movimientos_economicos()", connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var movimiento = new MovimientoEconomico
                                {
                                    MovimientoId = reader.GetInt32(reader.GetOrdinal("movimiento_id")),
                                    Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                    ContadorId = reader.GetInt32(reader.GetOrdinal("contador_id")),
                                    Tipo = reader.IsDBNull(reader.GetOrdinal("tipo")) ? null : reader.GetString(reader.GetOrdinal("tipo")),
                                    Cantidad = reader.IsDBNull(reader.GetOrdinal("cantidad")) ? 0 : reader.GetDecimal(reader.GetOrdinal("cantidad")),
                                    TorneoId = reader.GetInt32(reader.GetOrdinal("torneo_id")),
                                    Comprobante = reader.IsDBNull(reader.GetOrdinal("comprobante")) ? null : reader.GetString(reader.GetOrdinal("comprobante")), // Cambiado a "comprobante"
                                    Contador = reader.IsDBNull(reader.GetOrdinal("contador_nombre")) ? null : reader.GetString(reader.GetOrdinal("contador_nombre")), // Cambiado a "comprobante"
                                };

                                movimientos.Add(movimiento);
                            }
                        }
                    }
                }

                return movimientos;
            }
            catch
            {
                return movimientos;
            }
        }

        private List<PagoPartido> ObtenerPagos()
        {
            List<PagoPartido> pagos = new();
            try
            {

                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("Select * from obtener_pago_partido()", connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var pago = new PagoPartido
                                {
                                    PagoPartidoId = reader.GetInt32(reader.GetOrdinal("pago_partido_id")),
                                    FechaPago = reader.IsDBNull(reader.GetOrdinal("fecha_pago")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("fecha_pago")),
                                    EquipoId = reader.GetInt32(reader.GetOrdinal("equipo_id")),
                                    PartidoId = reader.GetInt32(reader.GetOrdinal("partido_id")),
                                    Completado = reader.GetBoolean(reader.GetOrdinal("completado")),
                                    Comprobante = reader.IsDBNull(reader.GetOrdinal("comprobante")) ? null : reader.GetString(reader.GetOrdinal("comprobante")), // Cambiado a "comprobante"
                                    Equipo = reader.IsDBNull(reader.GetOrdinal("nombre_equipo")) ? null : reader.GetString(reader.GetOrdinal("nombre_equipo")), 
                                    Partido = reader.IsDBNull(reader.GetOrdinal("nombre_partido")) ? null : reader.GetString(reader.GetOrdinal("nombre_partido")), // Cambiado a "comprobante"
                                };

                                pagos.Add(pago);
                            }
                        }
                    }
                }

                return pagos;
            }
            catch(Exception ex) {
                string mensaje = ex.Message;
                return pagos;
            }
        }


        public List<Usuario> ObtenerArbitros(string dia, TimeSpan hora)
        {
            var arbitros = new List<Usuario>();

            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM obtenerArbitrosDisponibles(@dia, @hora);", connection))
                {
                    cmd.Parameters.AddWithValue("dia", dia);
                    cmd.Parameters.AddWithValue("hora", hora);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var arbitro = new Usuario
                            {
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                ApPaterno = reader.GetString(reader.GetOrdinal("ap_paterno")),
                                ApMaterno = reader.GetString(reader.GetOrdinal("ap_materno")),
                                RolId = reader.GetInt32(reader.GetOrdinal("rol_id"))
                            };

                            arbitros.Add(arbitro);
                        }
                    }
                }
            }

            return arbitros;
        }

    }
}

