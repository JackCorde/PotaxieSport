namespace PotaxieSport.Models
{
    public class DetallePartido
    {
        public int PartidoId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Lugar { get; set; }
        public decimal Costo { get; set; }
        public string TipoEquipo { get; set; }
        public string NombreEquipoRetador { get; set; }
        public string NombreEquipoDefensor { get; set; }
        public string NombreEquipoGanador { get; set; }
        public int IdEquipoGanador { get; set; }
        public string NombreArbitro { get; set; }
    }
}
