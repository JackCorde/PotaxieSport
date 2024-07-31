namespace PotaxieSport.Models
{
    public class DisponibilidadArbitro
    {
        public int DispArbId { get; set; }
        public int UsuarioId { get; set; }
        public string? Dia { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFinal { get; set; }

    }
}
