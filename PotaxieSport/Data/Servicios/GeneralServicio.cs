using System.Data;
using Npgsql;
using PotaxieSport.Models;

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

        //Metodo Agregar Usuario
        //obtener roles
        public List<Rol> ObtenerRoles()
        {
            var roles = new List<Rol>();

            // Abre la conexión a la base de datos
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();

                // Crea el comando SQL para llamar a la función que obtiene roles
                using (var cmd = new NpgsqlCommand("SELECT * FROM obtener_roles()", connection))
                {
                    // Ejecuta el comando y lee los resultados
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Recorre los resultados
                        while (reader.Read())
                        {
                            roles.Add(new Rol
                            {
                                RolId = reader.GetInt32(reader.GetOrdinal("rol_id")), // Asegúrate de que el nombre de la columna sea correcto
                                RolNombre = reader.GetString(reader.GetOrdinal("rol_nombre")) // Asegúrate de que el nombre de la columna sea correcto
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


    }
}
