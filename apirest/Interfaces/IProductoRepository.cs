using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apirest.Models;

namespace apirest.interfaces
{
    //se llama la funcion obtener todos de Productorespository
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> ObtenerTodos();
        Task<Producto?> ObtenerPorId(int id);
        Task<int> crear(Producto producto);
        Task<bool> eliminar(int id);   
        Task<bool> actualizar(Producto producto);
        Task<bool> ExisteProducto(string nombre, string cliente);
    }
}   