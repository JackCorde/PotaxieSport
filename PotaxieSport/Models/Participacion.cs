namespace PotaxieSport.Models
{
    public class Participacion
    {
        public int ParticipacionId {  get; set; }
        public int EquipoId { get; set; }
        public string? Equipo { get; set;}
        public int TorneoId { get; set; }
        public string? Torneo { get; set;}
        public bool Activo { get; set; }
    }
}
