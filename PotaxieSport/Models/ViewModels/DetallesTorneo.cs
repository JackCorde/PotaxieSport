namespace PotaxieSport.Models.ViewModels
{
    public class DetallesTorneo
    {
       
        public Torneo torneo { get; set; }
        public List<Equipos> equipos { get; set; }
        public List<Partido> partidos { get; set; }

    }
}
