using System.Data;
using Npgsql;
using Npgsql.Internal;
using PotaxieSport.Data;
using PotaxieSport.Models;

namespace TorneosDeportivos.Data.Servicios
{
    public class GeneralServicio
    {
        private readonly Contexto _contexto;

        public GeneralServicio(Contexto contexto)
        {
            _contexto = contexto;
        }

        

        public void NumeroIntento(int id)
        {
            using (var connection = new NpgsqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("AgregarIntentoFallido", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
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
                using (NpgsqlCommand cmd = new NpgsqlCommand("LimpiarIntentoFallido", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery(); // Ejecutar la consulta sin retorno de datos
                }
                connection.Close(); // Cerrar la conexión después de terminar
            }
        }

    }
}
