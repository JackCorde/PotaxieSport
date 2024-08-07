namespace PotaxieSport.Models.ViewModels
{
    public class DetallesTorneo
    {
       
        public Torneo torneo { get; set; }
        public List<Equipo> equipos { get; set; }
        public List<Equipo> equiposNoInscritos {  get; set; }
        public List<Partido> partidos { get; set; }

    }
}
