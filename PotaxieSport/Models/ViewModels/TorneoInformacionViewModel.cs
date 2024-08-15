namespace PotaxieSport.Models.ViewModels
{
    public class TorneoInformacionViewModel
    {
        public Torneo Torneo { get; set; }
        public List<Equipo> Equipos { get; set; }
        public List<Participacion> Participaciones { get; set; }
    }
}
