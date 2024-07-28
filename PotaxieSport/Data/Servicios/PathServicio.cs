using System.Data;
using System.Data.SqlClient;
using PotaxieSport.Models;

namespace PotaxieSport.Data.Servicios
{
    public class PathServicio
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly Contexto _contexto;

        public PathServicio(IWebHostEnvironment hostingEnvironment,Contexto contexto)
        {
            _hostingEnvironment = hostingEnvironment;
             _contexto = contexto;
        }

        /*Archivos*/
        public string PathPartidoCedula()
        {
            var PathFinal = Path.Combine(_hostingEnvironment.WebRootPath, "Formatos", "Cedulas");
            return PathFinal;

        }
        public string PathContaduriaComprobantes()
        {
            var PathFinal = Path.Combine(_hostingEnvironment.WebRootPath, "Formatos", "Comprobantes");
            return PathFinal;

        }
        public string PathPagosPartidos()
        {
            var PathFinal = Path.Combine(_hostingEnvironment.WebRootPath, "Formatos", "Comprobantes", "PagoPartido");
            return PathFinal;

        }
        /*Imagenes*/
        public string PathTorneoImagen()
        {
            var PathFinal = Path.Combine(_hostingEnvironment.WebRootPath, "Formatos", "Imagenes", "Torneo");
            return PathFinal;

        }
        public string PathJugadorImagen()
        {
            var PathFinal = Path.Combine(_hostingEnvironment.WebRootPath, "Formatos", "Imagenes", "Jugadores");
            return PathFinal;

        }
    }
}
