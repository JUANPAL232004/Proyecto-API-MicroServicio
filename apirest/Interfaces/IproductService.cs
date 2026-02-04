using apirest.DTOs; 

namespace apirest.Interfaces 
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoResponse>> ObtenerTodos();
        Task<ProductoResponse> ObtenerPorId(int id);
        Task<int> CrearNuevoProducto(ProductoCreate dto);
        Task<bool> Actualizar(int id, ProductoCreate dto);
        Task<bool> Eliminar(int id);
    }
}