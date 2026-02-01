using System.ComponentModel.DataAnnotations;

namespace apirest.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nombre demasiado corto o largo")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "No se permiten caracteres especiales")]
        public required string NombreProducto { get; set; }

        [Required]
        public required string Cliente { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        public int IdEstado { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; } = DateTime.Now;
    }

    public class Usuario
    {   
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Nombre de usuario obligatorio")]
        [StringLength(100, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "No se permiten caracteres especiales")]
        public required string NombreUsuario { get; set; }
        
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [StringLength(150)]
        public required string CorreoElectronico { get; set; }
    }

    public class Estado
    {
        public int IdEstado { get; set; }
        public string? NombreEstado { get; set; }
    }
}