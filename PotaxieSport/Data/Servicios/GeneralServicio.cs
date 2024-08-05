using Npgsql;
using PotaxieSport.Models;
using System.Data;

namespace PotaxieSport.Data.Servicios
{
    public class GeneralServicio
    {
        private readonly Contexto _contexto;

        public GeneralServicio(Contexto contexto)
        {
            _contexto = contexto;
        }

        //Categorias 
        public List<Categoria> ObtenerCategorias()
        {
            var categorias = new List<Categoria>();

            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM obtener_categorias()", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var categoria = new Categoria
                            {
                                CategoriaId = reader.GetInt32(reader.GetOrdinal("categoria_id")),
                                CategoriaNombre = reader.GetString(reader.GetOrdinal("categoria_nombre")),
                                Rango = reader.GetString(reader.GetOrdinal("rango"))
                            };

                            categorias.Add(categoria);
                        }
                    }
                }
            }

            return categorias;
        }
        public Categoria ObtenerCategoriaPorId(int categoriaId)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM categoria WHERE categoria_id = @categoriaId", connection))
                {
                    command.Parameters.AddWithValue("@categoriaId", categoriaId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Categoria
                            {
                                CategoriaId = reader.GetInt32(reader.GetOrdinal("categoria_id")),
                                CategoriaNombre = reader.GetString(reader.GetOrdinal("Categoria_nombre"))
                            };
                        }
                    }
                }
            }

            return null;
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

        internal Usuario ConseguirUsuario(string correo)
        {
            Usuario usuario =
                null;
            try
            {
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM ValidarUsuario(@p_correo)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_correo", correo);

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

        //Metodo Agregar Usuario
        //obtener roles
        public List<Rol> ObtenerRoles()
        {
            var roles = new List<Rol>();

            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM obtener_roles()", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(new Rol
                            {
                                RolId = reader.GetInt32(reader.GetOrdinal("rol_id")), 
                                RolNombre = reader.GetString(reader.GetOrdinal("rol_nombre")) 
                            });
                        }
                    }
                }
            }

            return roles;
        }


        //verifucar email
        public bool EmailExists(string email)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT COUNT(1) FROM usuario WHERE email = @p_email", connection))
                {
                    cmd.Parameters.AddWithValue("p_email", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public void CrearUsuario(string nombre, string apPaterno, string apMaterno, string username, string email, int rolId, int errorAutentificacion, string password)
        {
            // Hashear la contraseña
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                try
                {
                    using (var cmd = new NpgsqlCommand("SELECT crearUsuario(@p_nombre, @p_ap_paterno, @p_ap_materno, @p_username, @p_email, @p_rol_id, @p_error_autentificacion, @p_password)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                        cmd.Parameters.AddWithValue("p_nombre", nombre);
                        cmd.Parameters.AddWithValue("p_ap_paterno", apPaterno);
                        cmd.Parameters.AddWithValue("p_ap_materno", apMaterno);
                        cmd.Parameters.AddWithValue("p_username", username);
                        cmd.Parameters.AddWithValue("p_email", email);
                        cmd.Parameters.AddWithValue("p_rol_id", rolId);
                        cmd.Parameters.AddWithValue("p_error_autentificacion", errorAutentificacion);
                        cmd.Parameters.AddWithValue("p_password", hashedPassword);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (PostgresException ex) when (ex.SqlState == "23505")
                {
                    throw new Exception("Correo ya existe.");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        //ActualizarUsuarios
        public Usuario ObtenerUsuarioPorId(int id)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                try
                {
                    using (var cmd = new NpgsqlCommand("SELECT * FROM obtenerUsuarioPorId(@p_id)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Usuario
                                {
                                    UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id")),
                                    Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                    ApPaterno = reader.GetString(reader.GetOrdinal("ap_paterno")),
                                    ApMaterno = reader.GetString(reader.GetOrdinal("ap_materno")),
                                    Username = reader.GetString(reader.GetOrdinal("username")),
                                    Email = reader.GetString(reader.GetOrdinal("email")),
                                    RolId = reader.GetInt32(reader.GetOrdinal("rol_id"))
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el usuario: " + ex.Message);
                }
                return null;
            }
        }

        public void ActualizarUsuario(int usuarioId, string nombre, string apPaterno, string apMaterno, string username, string email, int rolId)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                try
                {
                    using (var cmd = new NpgsqlCommand("SELECT actualizar_usuario(@p_usuario_id, @p_nombre, @p_ap_paterno, @p_ap_materno, @p_username, @p_email, @p_rol_id)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_usuario_id", usuarioId);
                        cmd.Parameters.AddWithValue("p_nombre", nombre);
                        cmd.Parameters.AddWithValue("p_ap_paterno", apPaterno);
                        cmd.Parameters.AddWithValue("p_ap_materno", apMaterno);
                        cmd.Parameters.AddWithValue("p_username", username);
                        cmd.Parameters.AddWithValue("p_email", email);
                        cmd.Parameters.AddWithValue("p_rol_id", rolId);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar el usuario: " + ex.Message);
                }
            }
        }

        // Disponibilidad De Arbitro
        public List<DisponibilidadArbitro> ObtenerDisponibilidadArbitro(int usuarioId)
        {
            var disponibilidades = new List<DisponibilidadArbitro>();

            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM obtener_disponibilidad_arbitro(@usuarioId)", connection))
                {
                    command.Parameters.AddWithValue("usuarioId", usuarioId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var disponibilidad = new DisponibilidadArbitro
                            {
                                DispArbId = reader.GetInt32(reader.GetOrdinal("disp_arb_id")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id")),
                                Dia = reader.GetString(reader.GetOrdinal("dia")),
                                HoraInicio = reader.GetTimeSpan(reader.GetOrdinal("hora_inicio")),
                                HoraFinal = reader.GetTimeSpan(reader.GetOrdinal("hora_final"))
                            };

                            disponibilidades.Add(disponibilidad);
                        }
                    }
                }
            }

            return disponibilidades;
        }
        //Agregar Disponibilidad
        public void AgregarDisponibilidadArbitro(DisponibilidadArbitro disponibilidad)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT agregar_disponibilidad_arbitro(@usuarioId, @dia, @horaInicio, @horaFinal)", connection))
                {
                    command.Parameters.AddWithValue("usuarioId", disponibilidad.UsuarioId);
                    command.Parameters.AddWithValue("dia", disponibilidad.Dia);
                    command.Parameters.AddWithValue("horaInicio", NpgsqlTypes.NpgsqlDbType.Time, disponibilidad.HoraInicio);
                    command.Parameters.AddWithValue("horaFinal", NpgsqlTypes.NpgsqlDbType.Time, disponibilidad.HoraFinal);

                    command.ExecuteNonQuery();
                }
            }
        }

        //Equipos
        public int AgregarEquipo(string nombreEquipo, string genero, int categoriaId, int usuarioCoach)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT public.agregar_equipo(@nombreEquipo, @genero, @categoriaId, @usuarioCoach)", connection))
                {
                    command.Parameters.AddWithValue("@nombreEquipo", nombreEquipo);
                    command.Parameters.AddWithValue("@genero", genero);
                    command.Parameters.AddWithValue("@categoriaId", categoriaId);
                    command.Parameters.AddWithValue("@usuarioCoach", usuarioCoach);

                    var equipoId = (int)command.ExecuteScalar();
                    return equipoId;
                }
            }
        }

        public Equipo ObtenerEquipoPorId(int equipoId)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                try
                {
                    using (var cmd = new NpgsqlCommand("SELECT * FROM obtener_equipo_por_id(@param_id)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("param_id", equipoId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Equipo
                                {
                                    EquipoId = reader.GetInt32(reader.GetOrdinal("equipo_id")),
                                    EquipoNombre = reader.GetString(reader.GetOrdinal("nombre_equipo")),
                                    Genero = reader.GetString(reader.GetOrdinal("genero")),
                                    Logo = reader.GetString(reader.GetOrdinal("logo")),
                                    CategoriaId = reader.GetInt32(reader.GetOrdinal("categoria_id")),
                                    UsuarioCoachId = reader.GetInt32(reader.GetOrdinal("usuario_coach")),
                                    TorneoActualId = reader.IsDBNull(reader.GetOrdinal("torneo_actual")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("torneo_actual"))
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el equipo: " + ex.Message);
                }
                return null;
            }
        }


        //Obtner Jugadores por id de equipo
        public List<Jugador> ObtenerJugadoresPorEquipoId(int equipoId)
        {
            var jugadores = new List<Jugador>();

            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM obtener_jugadores_por_equipo_id(@equipoId)", connection))
                {
                    command.Parameters.AddWithValue("@equipoId", equipoId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            jugadores.Add(new Jugador
                            {
                                JugadorId = reader.GetInt32(reader.GetOrdinal("jugador_id")),
                                JugadorNombre = reader.GetString(reader.GetOrdinal("nombre")),
                                ApPaterno = reader.GetString(reader.GetOrdinal("ap_paterno")),
                                ApMaterno = reader.GetString(reader.GetOrdinal("ap_materno")),
                                Edad = reader.GetInt32(reader.GetOrdinal("edad")),
                                Fotografia = reader.GetString(reader.GetOrdinal("fotografia")),
                                EquipoId = reader.GetInt32(reader.GetOrdinal("equipo_id")),
                                Posicion = reader.GetString(reader.GetOrdinal("posicion")),
                                NumJugador = reader.GetInt32(reader.GetOrdinal("num_jugador")),
                                Username = reader.GetString(reader.GetOrdinal("username"))
                            });
                        }
                    }
                }
            }

            return jugadores;
        }

        public void CrearJugador(Jugador jugador)
        {
            // Hashear la contraseña (si es necesario)
            string hashedContrasena = BCrypt.Net.BCrypt.HashPassword(jugador.Clave);

            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                try
                {
                    using (var cmd = new NpgsqlCommand("SELECT public.crear_jugador(@p_nombre, @p_ap_paterno, @p_ap_materno, @p_edad, @p_fotografia, @p_equipo_id, @p_posicion, @p_num_jugador, @p_username, @p_contrasena)", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("p_nombre", jugador.JugadorNombre);
                        cmd.Parameters.AddWithValue("p_ap_paterno", jugador.ApPaterno);
                        cmd.Parameters.AddWithValue("p_ap_materno", jugador.ApMaterno);
                        cmd.Parameters.AddWithValue("p_edad", jugador.Edad);
                        cmd.Parameters.AddWithValue("p_fotografia", jugador.Fotografia);
                        cmd.Parameters.AddWithValue("p_equipo_id", jugador.EquipoId);
                        cmd.Parameters.AddWithValue("p_posicion", jugador.Posicion);
                        cmd.Parameters.AddWithValue("p_num_jugador", jugador.NumJugador);
                        cmd.Parameters.AddWithValue("p_username", jugador.Username);
                        cmd.Parameters.AddWithValue("p_contrasena", hashedContrasena);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (PostgresException ex) when (ex.SqlState == "23505")
                {
                    throw new Exception("Error al insertar el jugador: posible violación de restricción única.");
                }
                finally
                {
                    connection.Close();
                }
            }
        }



    }
}
