namespace PotaxieSport.Models
{
    public class RegistroSalud
    {
        public int RegistroSaludId { get; set; }
        public int JugadorId { get; set; }
        public string? Jugador { get; set; }
        public int FrecuenciaCardiaca { get; set; }
        public string? Estatus { get; set; }
        public DateTime Fecha { get; set; }
    }
}
