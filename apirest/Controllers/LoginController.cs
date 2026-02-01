using Microsoft.AspNetCore.Mvc;
using apirest.Interfaces;
using apirest.Models;
using Microsoft.IdentityModel.Tokens; 
using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims;
using System.Text; 
using System.ComponentModel.DataAnnotations;

namespace apirest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginRepository _repository;
        private readonly IConfiguration _config; 
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILoginRepository repository, IConfiguration config, ILogger<AuthController> logger)
        {
            _repository = repository;
            _config = config; 
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var usuario = await _repository.ObtenerPorEmail(model.CorreoElectronico);

            // Verificamos si el usuario existe y si la contraseña coincide con el Hash
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password))
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas" });
            }

            var token = GenerarToken(usuario);

            return Ok(new 
            { 
                mensaje = "Login exitoso", 
                token = token, 
                usuario = usuario.NombreUsuario 
            });
        }

       [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegisterRequest model)
        {
            var usuarioExistente = await _repository.ObtenerPorEmail(model.CorreoElectronico);
            if (usuarioExistente != null) 
                return BadRequest(new { mensaje = "El correo ya está registrado" });

            var nuevoUsuario = new Usuario
            {
                NombreUsuario = model.NombreUsuario,
                CorreoElectronico = model.CorreoElectronico,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            bool creado = await _repository.Registrar(nuevoUsuario);
            if (creado) 
            {
                // En una API real, aquí podrías devolver la URL del nuevo recurso
                return CreatedAtAction(nameof(Login), new { email = nuevoUsuario.CorreoElectronico }, new { mensaje = "Usuario registrado correctamente" });
            }

            return StatusCode(500, new { mensaje = "Error interno al intentar registrar" });
        }
        private string GenerarToken(Usuario usuario)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                new Claim("UsuarioId", usuario.IdUsuario.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(tokenConfig);
        }
    }

    // --- CLASES DE PETICIÓN (DTOs) ---

        public class LoginRequest
        {
            [Required(ErrorMessage = "El correo es obligatorio")]
            [EmailAddress]
            public required string CorreoElectronico { get; set; }

            [Required(ErrorMessage = "La contraseña es obligatoria")]
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