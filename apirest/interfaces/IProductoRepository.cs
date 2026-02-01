using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apirest.models;

namespace apirest.interfaces
{
    //se llama la funcion obtener todos de Productorespository
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> ObtenerTodos();
    }
}