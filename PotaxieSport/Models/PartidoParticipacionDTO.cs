namespace PotaxieSport.Models
{
    public class PartidoParticipacionDTO
    {
        public int EquipoId { get; set; }
        public string NombreEquipo { get; set; }
        public int? ParticipacionId { get; set; }
        public bool? ParticipacionActivo { get; set; }
        public int? PartidoId { get; set; }
        public int? TorneoIdPartido { get; set; }
        public int? EquipoRetador { get; set; }
        public int? EquipoDefensor { get; set; }
        public int? EquipoGanador { get; set; }
        public int? UsuarioArbitro { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Lugar { get; set; }
        public decimal Costo { get; set; }
        public string EquipoRetadorNombre { get; set; }
        public string EquipoDefensorNombre { get; set; }
    }
}

