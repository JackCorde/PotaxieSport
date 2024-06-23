namespace PotaxieSport.Models
{
    public class Jugador
    {
        public int JugadorId { get; set; }
        public string? JugadorNombre { get; set; }
        public string? ApPaterno { get; set; }
        public string? ApMaterno { get; set; }
        public int Edad { get; set; }
        public string? Fotografia { get; set; }
        public int EquipoId { get; set; }
        public string? Equipo { get; set; }
        public string? Posicion { get; set; }
        public int NumJugador { get; set; }
    }
}
