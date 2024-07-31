namespace PotaxieSport.Models.ViewModels
{
    public class Request
    {
        public int EquipoId { get; set; }
        public int JugadorId { get; set; }
        public int FrecuenciaCard { get; set; }

    }

    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class LoginDoctor
    {
        public string password { get; set; }
        public string correo { get; set; }
    }

    public class DatosJugador
    {
        public Jugador jugador { get; set; }
        public List<RegistroSalud> registros {get; set;}
        public Torneo? torneo { get; set; }
    }


    public class DatosDoctor
    {
        public Usuario doctor { get; set; }
        public List<Torneos> torneos { get; set; }

    }

    public class Torneos
    {
        public Torneo torneo { get; set; }
        public List<Equipos> equipos { get; set;}
    }

    public class Equipos
    {
        public Equipo equipo { get; set; }
        public List<Jugadores> jugadores { get; set; }
    }

    public class Jugadores
    {
        public Jugador jugador { get; set; }
        public List<RegistroSalud> registros { get; set; }
    }
}
