using PotaxieSport.Models.ViewModels;

namespace PotaxieSport.Models
{
    public class PartidoParticipacionDTO
    {
        public Torneo torneo { get; set; }
        public List<Equipos> equipos { get; set; }
        public List<Partido> partidos { get; set; }
        public List<Participacion> participacions { get; set; }
    }

}
