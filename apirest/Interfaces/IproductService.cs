using apirest.DTOs;
using apirest.Models;

namespace apirest.Interfaces 
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> ObtenerTodos();
        Task<ProductoResponse?> ObtenerPorId(int id);
        Task<int> CrearNuevoProducto(ProductoCreate dto);
        Task<bool> Actualizar(int id, ProductoCreate dto);
        Task<bool> Eliminar(int id);
    }
}