using Npgsql;
using PotaxieSport.Models;
using System.Data;
using System.Data.Common;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


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
        public async Task CrearJugadorAsync(string nombre, string apPaterno, string apMaterno, int edad, string fotografia, int equipoId, string posicion, int numJugador, string username, string clave)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_contexto.Conexion))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand("SELECT crear_jugador(@p_nombre, @p_ap_paterno, @p_ap_materno, @p_edad, @p_fotografia, @p_equipo_id, @p_posicion, @p_num_jugador, @p_username, @p_contrasena)", connection))
                    {
                        command.Parameters.AddWithValue("p_nombre", nombre);
                        command.Parameters.AddWithValue("p_ap_paterno", apPaterno);
                        command.Parameters.AddWithValue("p_ap_materno", apMaterno);
                        command.Parameters.AddWithValue("p_edad", edad);
                        command.Parameters.AddWithValue("p_fotografia", fotografia);
                        command.Parameters.AddWithValue("p_equipo_id", equipoId);
                        command.Parameters.AddWithValue("p_posicion", posicion);
                        command.Parameters.AddWithValue("p_num_jugador", numJugador);
                        command.Parameters.AddWithValue("p_username", username);
                        command.Parameters.AddWithValue("p_contrasena", clave);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                // Captura y maneja excepciones específicas de Npgsql
                Console.WriteLine($"Error de Npgsql: {npgsqlEx.Message}");
                throw; // Relanza la excepción para que pueda ser manejada por el controlador
            }
            catch (Exception ex)
            {
                // Captura cualquier otra excepción
                Console.WriteLine($"Error al crear el jugador: {ex.Message}");
                throw; // Relanza la excepción para que pueda ser manejada por el controlador
            }
        }


        //Obtener tordeos po id de coach
        public List<Torneo> ObtenerTorneosPorCoach(int coachId)
        {
            List<Torneo> torneos = new List<Torneo>();

            using (var conn = new NpgsqlConnection(_contexto.Conexion))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM obtener_torneos_por_coach(@coachId)", conn))
                {
                    cmd.Parameters.AddWithValue("coachId", coachId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var torneo = new Torneo
                            {
                                TorneoId = reader.GetInt32(0),
                                NombreTorneo = reader.GetString(1),
                                CategoriaId = reader.GetInt32(2),
                                Genero = reader.GetString(3),
                                Logo = reader.IsDBNull(4) ? null : reader.GetString(4),
                                AdministradorId = reader.GetInt32(5),
                                ContadorId = reader.IsDBNull(6) ? 0 : reader.GetInt32(6), // ContadorId puede ser null
                                DoctorId = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),   // DoctorId puede ser null
                                FechaInicio = reader.GetDateTime(8),
                                FechaFin = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9)
                            };

                            torneos.Add(torneo);
                        }
                    }
                }
            }

            return torneos;
        }

        public Equipo GetEquipoByCoachAndTorneo(int torneoId, int userId)
        {
            Equipo equipo = null;

            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM get_equipo_by_coach_and_torneo(@p_usuario_coach, @p_torneo_actual)", connection))
                {
                    command.Parameters.AddWithValue("p_usuario_coach", userId);
                    command.Parameters.AddWithValue("p_torneo_actual", torneoId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            equipo = new Equipo
                            {
                                EquipoId = reader.GetInt32(0),
                                EquipoNombre = reader.GetString(1),
                                Genero = reader.GetString(2),
                                Logo = reader.GetString(3),
                                CategoriaId = reader.GetInt32(4),
                                UsuarioCoachId = reader.GetInt32(5),
                                TorneoActualId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6)
                            };
                        }
                    }
                }
            }

            return equipo;
        }

        public List<DetallePartido> ObtenerDetallesPartido(int equipoId)
        {
            var detallesPartido = new List<DetallePartido>();

            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM detalles_partido(@p_equipo_id)", connection))
                {
                    command.Parameters.AddWithValue("p_equipo_id", equipoId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var detalle = new DetallePartido
                            {
                                PartidoId = reader.GetInt32(reader.GetOrdinal("partido_id")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                Hora = reader.GetTimeSpan(reader.GetOrdinal("hora")),
                                Lugar = reader.GetString(reader.GetOrdinal("lugar")),
                                Costo = reader.GetDecimal(reader.GetOrdinal("costo")),
                                TipoEquipo = reader.GetString(reader.GetOrdinal("tipo_equipo")),
                                NombreEquipoRetador = reader.GetString(reader.GetOrdinal("nombre_equipo_retador")),
                                NombreEquipoDefensor = reader.GetString(reader.GetOrdinal("nombre_equipo_defensor")),
                                NombreEquipoGanador = reader.GetString(reader.GetOrdinal("nombre_equipo_ganador")),
                                IdEquipoGanador = reader.GetInt32(reader.GetOrdinal("id_equipo_ganador")),
                                NombreArbitro = reader.GetString(reader.GetOrdinal("nombre_arbitro"))
                            };
                            detallesPartido.Add(detalle);
                        }
                    }
                }
            }

            return detallesPartido;
        }



        public int CrearTorneo(Torneo torneo)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();

                // Comando SQL para insertar un nuevo torneo y obtener el ID generado
                var query = @"
        INSERT INTO public.torneo (
            nombre_torneo,
            categoria_id,
            genero,
            usuario_admin,
            usuario_contador,
            usuario_doctor,
            fecha_inicio,
            fecha_fin
        )
        VALUES (
            @NombreTorneo,
            @CategoriaId,
            @Genero,
            @AdministradorId,
            @ContadorId,
            @DoctorId,
            @FechaInicio,
            @FechaFin
        )
        RETURNING torneo_id;"; // Devolver el ID del torneo recién insertado

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombreTorneo", torneo.NombreTorneo);
                    command.Parameters.AddWithValue("@CategoriaId", torneo.CategoriaId);
                    command.Parameters.AddWithValue("@Genero", torneo.Genero);
                    command.Parameters.AddWithValue("@AdministradorId", torneo.AdministradorId);
                    command.Parameters.AddWithValue("@ContadorId", torneo.ContadorId);
                    command.Parameters.AddWithValue("@DoctorId", torneo.DoctorId);
                    command.Parameters.AddWithValue("@FechaInicio", torneo.FechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", torneo.FechaFin);

                    // Ejecutar el comando y obtener el ID del torneo recién insertado
                    var torneoId = (int)command.ExecuteScalar();
                    return torneoId;
                }
            }
        }




        public int InsertarJugador(Jugador jugador)
        {
            using var connection = new NpgsqlConnection(_contexto.Conexion);
            connection.Open();

            using var command = new NpgsqlCommand(@"
            INSERT INTO public.jugador (nombre, ap_paterno, ap_materno, edad, fotografia, equipo_id, posicion, num_jugador, username, contrasena)
            VALUES (@nombre, @apPaterno, @apMaterno, @edad, @fotografia, @equipoId, @posicion, @numJugador, @username, @clave)
            RETURNING jugador_id", connection);

            command.Parameters.AddWithValue("@nombre", jugador.JugadorNombre ?? string.Empty);
            command.Parameters.AddWithValue("@apPaterno", jugador.ApPaterno ?? string.Empty);
            command.Parameters.AddWithValue("@apMaterno", jugador.ApMaterno ?? string.Empty);
            command.Parameters.AddWithValue("@edad", jugador.Edad);
            command.Parameters.AddWithValue("@fotografia", string.IsNullOrEmpty(jugador.Fotografia) ? string.Empty : jugador.Fotografia);
            command.Parameters.AddWithValue("@equipoId", jugador.EquipoId);
            command.Parameters.AddWithValue("@posicion", jugador.Posicion ?? string.Empty);
            command.Parameters.AddWithValue("@numJugador", jugador.NumJugador);
            command.Parameters.AddWithValue("@username", jugador.Username ?? string.Empty);
            command.Parameters.AddWithValue("@clave", jugador.Clave ?? string.Empty);

            return (int)command.ExecuteScalar(); // Devuelve el ID del jugador insertado
        }

    }
}
