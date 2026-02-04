// 1. Asegúrate de que este nombre sea el mismo que pusiste 
//    dentro de los archivos de la carpeta DTOs
using apirest.DTOs; 

namespace apirest.Interfaces 
{
    public interface IProductoService
    {
        // Si aquí ProductoResponseDTO sigue en rojo, 
        // es que el "using" de arriba no está encontrando la clase.
        Task<IEnumerable<ProductoResponse>> ObtenerTodos();
        Task<ProductoResponse> ObtenerPorId(int id);
        Task<int> CrearNuevoProducto(ProductoCreate dto);
        Task<bool> Actualizar(int id, ProductoCreate dto);
        Task<bool> Eliminar(int id);
    }
}