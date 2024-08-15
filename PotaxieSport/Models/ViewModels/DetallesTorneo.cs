namespace PotaxieSport.Models.ViewModels
{
    public class DetallesTorneo
    {
       
        public Torneo torneo { get; set; }
        public List<Equipo> equipos { get; set; }
        public List<Equipo> equiposNoInscritos {  get; set; }
        public List<Partidos> partidos { get; set; }
        public List<MovimientoEconomico> movimientos { get; set; }

    }

    public class Partidos
    {
        public Partido partido { get; set; }
        public List<PagoPartido> pagos { get; set; }
    }
}
