using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apirest.DTOs
{
    public class ProductoEliminar
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? NombreEstado { get; set; } 
        public DateTime Fecha { get; set; }
    }
}
