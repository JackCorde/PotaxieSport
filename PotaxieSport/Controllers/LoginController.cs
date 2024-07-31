using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PotaxieSport.Data;
using PotaxieSport.Data.Servicios;
using PotaxieSport.Models;
using PotaxieSport.Models.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PotaxieSport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly GeneralServicio _generalServicio;
        public LoginController(IConfiguration config, Contexto contexto)
        {
            _config = config;
            _generalServicio = new GeneralServicio(contexto);
        }

        [HttpGet]
        public IActionResult Get()
        {
            Usuario currentUser = GetCurrentUser();
            return Ok($"Hola {currentUser.Nombre}, tu eres un {currentUser.Rol}");
        }

        [HttpPost]
        public IActionResult Login(UsuarioLogin userLogin)
        {
            Usuario user = Authenticate(userLogin);
            if (user != null)
            {
                //crear el token
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("Usuario No Encontrado");
        }

        private Usuario Authenticate(UsuarioLogin userLogin)
        {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            Usuario currentUser = _generalServicio.ConseguirUsuario(userLogin.Username);

            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }

        private string Generate(Usuario user)
        {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //crear claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Nombre),
                new Claim(ClaimTypes.Surname, user.ApPaterno),
                new Claim(ClaimTypes.Role, user.Rol)
            };

            //crear token
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Usuario GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new Usuario
                {
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Nombre = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    ApPaterno = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Rol = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}
