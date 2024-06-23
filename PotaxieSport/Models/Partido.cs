namespace PotaxieSport.Models
{
    public class Partido
    {
        public int PartidoId { get; set; }
        public int TorneoId { get; set; }
        public string? Torneo { get; set; }
        public int EquipoRetadorId { get; set; }
        public string? EquipoRetador { get; set; }
        public int EquipoDefensorId { get; set; }
        public string? EquipoDefensor { get; set; }
        public int EquipoGanadorId { get; set; }
        public string? EquipoGanador { get; set; }
        public int UsuarioArbitro {  get; set; }
        public string? Arbitro { get; set; }
        public string? Cedula { get; set; }
        public DateTime Fecha { get; set; }
        public int Hora { get; set; }
        public string? Lugar { get; set; }
        public double Costo { get; set; }

    }
}
