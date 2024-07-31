namespace PotaxieSport.Models
{
    public class Equipo
    {
        public int EquipoId { get; set; }
        public string? EquipoNombre { get; set; }
        public string? Genero { get; set; }
        public string? Logo { get; set; }
        public int CategoriaId { get; set; }
        public string? Categoria { get; set; }
        public int UsuarioCoachId { get; set; }
        public string? Coach { get; set; }

        public int TorneoActualId { get; set; }
        public string? TorneoActual { get; set; }

    }
}
