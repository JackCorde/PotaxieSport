using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Npgsql.Internal;
using PotaxieSport.Data;
using PotaxieSport.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Authorization;

namespace PotaxieSport.Data.Servicios
{
    public class ArchivosServicio
    {
        private readonly Contexto _contexto;
        private readonly PathServicio _PathServicio;

        public ArchivosServicio(Contexto contexto, IWebHostEnvironment hostingEnvironment)
        {
            _contexto = contexto;
            _PathServicio = new PathServicio(hostingEnvironment, contexto);
        }

        // ... Código para subir Imagenes y Archivos ...
        
        
        [Authorize]
        [RequestSizeLimit(200 * 1024 * 1024)]
        //Ingresa el archivo y el ID del torneo.
        /*
            Estructura del nombre de imagen torneo: torneoId + "T_" + file.FileName 
            Estructura del nombre de imagen jugador: jugadorId + "J_" + file.FileName
            Estructura del nombre de comprobante transacción: movimientoEconomicoId + "CME_" + file.FileName
            Estructura del nombre de comprobante Pago Partido: pagoPartidoId + "CPP_" + file.FileName
            Estructura del nombre de cedula Partido: partidoId + "CP_" + file.FileName
        */
        public string SubirArchivo(IFormFile file, string nombreArchivo, string elemento)
        {
            if (file != null && file.Length > 0)
            {
                switch (elemento)
                {
                    //Para Fotografías de Torneos
                    case "Torneo":
                        try
                        {
                            // Obtiene la ruta de la carpeta de subida
                            var uploadsFolder = _PathServicio.PathTorneoImagen();

                            // Combina la carpeta de subida con el nombre del archivo
                            var filePath = Path.Combine(uploadsFolder, nombreArchivo);

                            // Verifica si la carpeta de subida existe, si no, la crea
                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            // Verifica si un archivo con el mismo nombre ya existe
                            if (System.IO.File.Exists(filePath))
                            {
                                return "Ya existe un archivo con este nombre.";
                            }

                            // Usa FileStream para crear el archivo y copiar el contenido
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }

                            return "Archivo subido con éxito.";
                        }
                        catch (Exception ex)
                        {
                            // Maneja cualquier excepción que pueda ocurrir
                            return $"Ocurrió un error al subir el archivo: {ex.Message}";
                        }

                    //Para Fotografías del Equipo
                    case "Equipo":
                        try
                        {
                            
                            var uploadsFolder = _PathServicio.PathEquipoImagen();

                            
                            var filePath = Path.Combine(uploadsFolder, nombreArchivo);

                            
                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            
                            if (System.IO.File.Exists(filePath))
                            {
                                return "Ya existe un archivo con este nombre.";
                            }

                            
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }

                            return "Archivo subido con éxito.";
                        }
                        catch (Exception ex)
                        {
                            
                            return $"Ocurrió un error al subir el archivo: {ex.Message}";
                        }

                    //Para Fotografías del Jugador
                    case "Jugador":
                        try
                        {
                            var uploadsFolder = _PathServicio.PathJugadorImagen();

                            var filePath = Path.Combine(uploadsFolder, nombreArchivo);

                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            if (System.IO.File.Exists(filePath))
                            {
                                return "Ya existe un archivo con este nombre.";
                            }

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            return "Archivo subido con éxito.";
                        }
                        catch (Exception ex)
                        {
                            return $"Ocurrió un error al subir el archivo: {ex.Message}";
                        }

                    // Para Comprobantes de Transacciones Económicas
                    case "Comprobante":
                        try
                        {

                            var uploadsFolder = _PathServicio.PathContaduriaComprobantes();

                            var filePath = Path.Combine(uploadsFolder, nombreArchivo);

                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            if (System.IO.File.Exists(filePath))
                            {
                                return "Ya existe un archivo con este nombre.";
                            }

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            return "Archivo subido con éxito.";
                        }
                        catch (Exception ex)
                        {
                            return $"Ocurrió un error al subir el archivo: {ex.Message}";
                        }

                    //Para Pagos de Partidos por Equipo
                    case "Pago":
                        try
                        {
                            var uploadsFolder = _PathServicio.PathPagosPartidos();

                            var filePath = Path.Combine(uploadsFolder, nombreArchivo);

                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            if (System.IO.File.Exists(filePath))
                            {
                                return "Ya existe un archivo con este nombre.";
                            }

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            return "Archivo subido con éxito.";
                        }
                        catch (Exception ex)
                        {
                            return $"Ocurrió un error al subir el archivo: {ex.Message}";
                        }

                    //Para archivo de resultados de partido
                    case "Cedula":
                        try
                        {
                            var uploadsFolder = _PathServicio.PathPartidoCedula();

                            var filePath = Path.Combine(uploadsFolder, nombreArchivo);

                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }

                            if (System.IO.File.Exists(filePath))
                            {
                                return "Ya existe un archivo con este nombre.";
                            }

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            return "Archivo subido con éxito.";
                        }
                        catch (Exception ex)
                        {
                            return $"Ocurrió un error al subir el archivo: {ex.Message}";
                        }

                    default:
                        return "No existe ese tipo de elemento";
                }
            }
            else
            {
                return "Por favor, selecciona un archivo válido.";
            }


        }


        // ... Actualizar el nombre del Archivo para la BD en Postgres ...

        [Authorize]
        public void GuardarArchivoFotoEnBD(string nombreArchivo, int elementoId, string elemento)
        {
            switch (elemento)
            {
                //Para Fotografías de Torneos
                case "Torneo":
                    using (NpgsqlConnection con = new NpgsqlConnection(_contexto.Conexion))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM cargarImagenTorneo(@torneoId, @nombre)", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@torneoId", elementoId);
                            cmd.Parameters.AddWithValue("@nombre", nombreArchivo);

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    break;

                //Para Fotografías de Equipos
                case "Equipo":
                    using (NpgsqlConnection con = new NpgsqlConnection(_contexto.Conexion))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM cargarImagenEquipo(@equipoId, @nombre)", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@equipoId", elementoId);
                            cmd.Parameters.AddWithValue("@nombre", nombreArchivo);

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    break;

                //Para Fotografías del Jugador
                case "Jugador":
                    using (NpgsqlConnection con = new NpgsqlConnection(_contexto.Conexion))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM cargarImagenJugador(@jugadorId, @foto)", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@jugadorId", elementoId);
                            cmd.Parameters.AddWithValue("@foto", nombreArchivo);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    break;

                // Para Comprobantes de Transacciones Económicas
                case "Comprobante":
                    using (NpgsqlConnection con = new NpgsqlConnection(_contexto.Conexion))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM cargarComprobante(@transaccionId, @archivoComprobante)", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@transaccionId", elementoId);
                            cmd.Parameters.AddWithValue("@archivoComprobante", nombreArchivo);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    break;

                //Para Pagos de Partidos por Equipo
                case "Pago":
                    using (NpgsqlConnection con = new NpgsqlConnection(_contexto.Conexion))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM cargarPagoPartido(@pagoId, @archivoComprobante)", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@pagoId", elementoId);
                            cmd.Parameters.AddWithValue("@archivoComprobante", nombreArchivo);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    break;

                //Para archivo de resultados de partido
                case "Cedula":
                    using (NpgsqlConnection con = new NpgsqlConnection(_contexto.Conexion))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM cargarCedula(@partidoId, @archivoCedula)", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@partidoId", elementoId);
                            cmd.Parameters.AddWithValue("@archivoCedula", nombreArchivo);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    break;

            }

            

        }

    }
}
