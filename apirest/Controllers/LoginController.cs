using Microsoft.AspNetCore.Mvc;
using apirest.Interfaces;
using apirest.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace apirest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginRepository _repository;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILoginRepository repository, ILogger<AuthController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var usuario = await _repository.ObtenerPorEmail(model.CorreoElectronico);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password))
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas" });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                new Claim("UsuarioId", usuario.IdUsuario.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok(new { mensaje = "Login exitoso", usuario = usuario.NombreUsuario });
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegisterRequest model)
        {
            var usuarioExistente = await _repository.ObtenerPorEmail(model.CorreoElectronico);
            if (usuarioExistente != null) 
                return BadRequest(new { mensaje = "El correo ya está registrado" });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var nuevoUsuario = new Usuario
            {
                NombreUsuario = model.NombreUsuario,
                CorreoElectronico = model.CorreoElectronico,
                Password = passwordHash
            };

            bool creado = await _repository.Registrar(nuevoUsuario);
            if (creado) return Ok(new { mensaje = "Usuario registrado correctamente" });

            return StatusCode(500, "Error interno al intentar registrar");
        }
    } // <-- ESTA LLAVE CIERRA EL CONTROLADOR

    // ESTAS CLASES DEBEN ESTAR FUERA DE LA CLASE CONTROLADORA
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public required string CorreoElectronico { get; set; }

        [Required]
        public required string Password { get; set; }
    }

    public class RegisterRequest
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public required string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public required string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public required string Password { get; set; }
    }
} 