using Microsoft.AspNetCore.Mvc;

namespace PotaxieSport.Controllers
{
    public class FormsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CrearUsuario(int rolId, string rol)
        {
            ViewBag.RolId = rolId;
            ViewBag.Rol = rol;

            return View("CrearUsuario", "Forms");
        }
    }
}
