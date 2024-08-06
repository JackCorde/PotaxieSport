namespace PotaxieSport.Models
{
    public class DetallesEquipoViewModel
    {
        public Equipo Equipo { get; set; }
        public List<Jugador> Jugadores { get; set; }
        public Categoria Categoria { get; set; }
        public Usuario Coach { get; set; }
    }
}