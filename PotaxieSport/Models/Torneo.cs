namespace PotaxieSport.Models
{
    public class Torneo
    {
        public int TorneoId { get; set; }
        public string? NombreTorneo { get; set; }
        public int CategoriaId { get; set; }
        public string? Categoria { get; set; }
        public string? Genero { get; set; }
        public string? Logo { get; set; }
        public int AdministradorId { get; set; }
        public string? Administrador { get; set; }
        public int ContadorId { get; set; }
        public string? Contador { get; set; }
        public int DoctorId { get; set; }
        public string? Doctor { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin {  get; set; }
        public bool EnPartido { get; set; }
    }
}
