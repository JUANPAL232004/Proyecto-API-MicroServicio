using System.ComponentModel.DataAnnotations;


namespace apirest.Models
{
    public class LoginRequest
    {
        [Required]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}