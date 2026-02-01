using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace apirest.models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public required string NombreProducto { get; set; }   
        public required string Cliente { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int IdEstado { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class Usuario
    {
        public int IdUsuario { get; set; }
        public required string NombreUsuario { get; set; }
        public required string Password { get; set; }
        public required string CorreoElectronico { get; set; }
    }

    public class Estado
    {
        public int IdEstado { get; set; }
        public string? NombreEstado { get; set; }
    }
}