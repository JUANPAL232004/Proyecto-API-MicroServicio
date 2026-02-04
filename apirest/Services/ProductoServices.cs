using apirest.Models;
using apirest.interfaces;
using apirest.DTOs;
using apirest.Interfaces;

namespace apirest.Services
{
    public class ProductoServices : IProductoService
    {
        private readonly IProductoRepository _repository;

        public ProductoServices(IProductoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductoResponse>> ObtenerTodos()
        {
            var productos = await _repository.ObtenerTodos();
            return productos.Select(p => new ProductoResponse
            {
                IdProducto = p.IdProducto,
                NombreProducto = p.NombreProducto,
                Cliente = p.Cliente,
                Precio = p.Precio,
                Stock = p.Stock,
                NombreEstado = p.NombreEstado,
                Fecha = p.Fecha
            });
        }

        public async Task<ProductoResponse?> ObtenerPorId(int id)
        {
            var p = await _repository.ObtenerPorId(id);
            if (p == null) return null;

            return new ProductoResponse
            {
                IdProducto = p.IdProducto,
                NombreProducto = p.NombreProducto,
                Cliente = p.Cliente,
                Precio = p.Precio,
                Stock = p.Stock,
                NombreEstado = p.NombreEstado,
                Fecha = p.Fecha
            };
        }

        public async Task<int> CrearNuevoProducto(ProductoCreate request)
    {
        // ✅ Validaciones de negocio
        if (string.IsNullOrWhiteSpace(request.NombreProducto))
            throw new ArgumentException("El nombre es obligatorio");

        if (request.Precio <= 0)
            throw new ArgumentException("El precio debe ser mayor a cero");

        // ✅ Convertimos DTO → Model
        var producto = new Producto
        {
            NombreProducto = request.NombreProducto,
            Cliente = request.Cliente,
            Precio = request.Precio,
            Stock = request.Stock,
            IdEstado = request.IdEstado,
            Fecha = DateTime.UtcNow
        };

        // ✅ Guardamos
        return await _repository.Crear(producto);
    }

        public async Task<bool> Actualizar(int id, ProductoCreate dto)
        {
            // ✅ CORREGIDO: Usamos 'dto' que es el parámetro del método
            if (dto.Precio <= 0) throw new ArgumentException("Precio inválido");

            var productoExistente = await _repository.ObtenerPorId(id);
            if (productoExistente == null) return false;

            var productoParaActualizar = new Producto
            {
                IdProducto = id,
                NombreProducto = dto.NombreProducto,
                Cliente = dto.Cliente,
                Precio = dto.Precio,
                Stock = dto.Stock,
                IdEstado = dto.IdEstado
            };

            return await _repository.Actualizar(productoParaActualizar);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _repository.Eliminar(id);
        }
    }
}