namespace PotaxieSport.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string? Nombre { get; set; }
        public string? ApPaterno { get; set; }
        public string? ApMaterno { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public byte[]? Password { get; set; }
        public int RolId { get; set; }
        public string? Rol {  get; set; }
        public int ErrorAutentificacion { get; set; }
    }
}
