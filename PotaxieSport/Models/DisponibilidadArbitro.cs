namespace PotaxieSport.Models
{
    public class DisponibilidadArbitro
    {
        public int DispArbId { get; set; }
        public int UsuarioId { get; set; }
        public string? Dia {  get; set; }
        public int HoraInicio {  get; set; }
        public int HoraFinal {  get; set; }

    }
}
