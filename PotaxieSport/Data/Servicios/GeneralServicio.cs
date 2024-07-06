﻿using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Npgsql.Internal;
using PotaxieSport.Data;
using PotaxieSport.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PotaxieSport.Data.Servicios
{
    public class GeneralServicio
    {
        private readonly Contexto _contexto;

        public GeneralServicio(Contexto contexto)
        {
            _contexto = contexto;
        }

        public List<Usuario> ObtenerUsuarios()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM usuario", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var usuario = new Usuario
                            {
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? null : reader.GetString(reader.GetOrdinal("nombre")),
                                ApPaterno = reader.IsDBNull(reader.GetOrdinal("ap_paterno")) ? null : reader.GetString(reader.GetOrdinal("ap_paterno")),
                                ApMaterno = reader.IsDBNull(reader.GetOrdinal("ap_materno")) ? null : reader.GetString(reader.GetOrdinal("ap_materno")),
                                Username = reader.IsDBNull(reader.GetOrdinal("username")) ? null : reader.GetString(reader.GetOrdinal("username")),
                                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                                RolId = reader.GetInt32(reader.GetOrdinal("rol_id")),
                                ErrorAutentificacion = reader.GetInt32(reader.GetOrdinal("error_autentificacion"))
                            };

                            usuarios.Add(usuario);
                        }
                    }
                }
            }

            return usuarios;
        }

        public List<Torneo> ObtenerTorneos()
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
                                    AdministradorId = reader.GetInt32(reader.GetOrdinal("usuario_admin")),
                                    Administrador = reader.IsDBNull(reader.GetOrdinal("administrador")) ? null : reader.GetString(reader.GetOrdinal("administrador")),
                                    ContadorId = reader.GetInt32(reader.GetOrdinal("usuario_contador")),
                                    Contador = reader.IsDBNull(reader.GetOrdinal("contador")) ? null : reader.GetString(reader.GetOrdinal("contador")),
                                    DoctorId = reader.GetInt32(reader.GetOrdinal("usuario_doctor")),
                                    Doctor = reader.IsDBNull(reader.GetOrdinal("doctor")) ? null : reader.GetString(reader.GetOrdinal("doctor")),
                                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
                                    FechaFin = reader.GetDateTime(reader.GetOrdinal("fecha_fin"))
                                };

                                torneos.Add(torneo);
                            }
                        }
                    }
                }

                return torneos;
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return torneos;
            }
        }


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
                                    Logo = reader.IsDBNull(reader.GetOrdinal("logo")) ? null : reader.GetString(reader.GetOrdinal("logo")),
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


        public List<Partido> ObtenerPartidos()
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

        public void NumeroIntento(int id)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM AgregarIntentoFallido(@id)", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery(); // Ejecutar la consulta sin retorno de datos
                }
                connection.Close(); // Cerrar la conexión después de terminar
            }
        }

        public void LimpiarNumeroIntento(int id)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM LimpiarIntentoFallido(@id)", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery(); // Ejecutar la consulta sin retorno de datos
                }
                connection.Close(); // Cerrar la conexión después de terminar
            }
        }

        internal Usuario ConseguirUsuario(string username)
        {
            Usuario usuario = null;
            try
            {
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM ObtenerUsuarioPorUsername(@p_username)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id")),
                                    Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? null : reader.GetString(reader.GetOrdinal("nombre")),
                                    ApPaterno = reader.IsDBNull(reader.GetOrdinal("ap_paterno")) ? null : reader.GetString(reader.GetOrdinal("ap_paterno")),
                                    ApMaterno = reader.IsDBNull(reader.GetOrdinal("ap_materno")) ? null : reader.GetString(reader.GetOrdinal("ap_materno")),
                                    Username = reader.IsDBNull(reader.GetOrdinal("username")) ? null : reader.GetString(reader.GetOrdinal("username")),
                                    Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
                                    Password = reader.IsDBNull(reader.GetOrdinal("password")) ? null : reader.GetString(reader.GetOrdinal("password")),
                                    RolId = reader.GetInt32(reader.GetOrdinal("rol_id")),
                                    Rol = reader.IsDBNull(reader.GetOrdinal("rol")) ? null : reader.GetString(reader.GetOrdinal("rol")),
                                    ErrorAutentificacion = reader.GetInt32(reader.GetOrdinal("error_autentificacion"))
                                };

                                
                            }
                        }
                    }
                }

                return usuario;

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
    }
}
