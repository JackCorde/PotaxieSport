namespace PotaxieSport.Models
{
    public class MovimientoEconomico
    {
        public int MovimientoId { get; set; }
        public DateTime Fecha { get; set; }
        public int ContadorId { get; set; }
        public string? Contador { get; set; }
        public string? Tipo { get; set; }
        public Decimal Cantidad { get; set; }
        public int TorneoId { get; set; }
        public string? Torneo { get; set; }
        public string? Comprobante { get; set; }
        
    }
}
