using System.ComponentModel.DataAnnotations;

namespace apirest.Models
{
    public class RegisterRequest
    {
        [Required]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}