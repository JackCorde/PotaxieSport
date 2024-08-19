namespace PotaxieSport.Models
{
    public class DetallesEquipoViewModel
    {
        public Equipo Equipo { get; set; }
        public List<Jugador> Jugadores { get; set; }
        public Categoria Categoria { get; set; }
        public Usuario Coach { get; set; }
        public Torneo Torneo { get; set; }
        public Jugador Jugador { get; set; }
        public Jugador NuevoJugador { get; set; } = new Jugador();
    }
}
