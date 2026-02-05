using System.ComponentModel.DataAnnotations;

namespace apirest.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int IdEstado { get; set; }
        public string? NombreEstado { get; set; } // Para el JOIN del repositorio
        public DateTime Fecha { get; set; }
    }

    public class Usuario
    {   
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
    }

    public class Estado
    {
        public int IdEstado { get; set; }
        public string? NombreEstado { get; set; }
    }
}