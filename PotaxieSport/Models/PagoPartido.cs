namespace PotaxieSport.Models
{
    public class PagoPartido
    {
        public int PagoPartidoId { get; set; }
        public int EquipoId { get; set; }
        public string? Equipo { get; set; }
        public int PartidoId { get; set; }
        public string? Partido { get; set; }
        public bool Completado { get; set; }
        public DateTime? FechaPago { get; set; }
        public string? Comprobante { get; set; }
    }
}
