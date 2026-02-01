using Microsoft.AspNetCore.Mvc;
using apirest.interfaces;
using apirest.Models;
using Microsoft.Extensions.Configuration;

namespace apirest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepository _repository;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(IProductoRepository repository, ILogger<ProductoController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            try 
            {
                var productos = await _repository.ObtenerTodos();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los datos de los productos");
                return StatusCode(500, "Error interno del servidor");
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Producto producto)
        {
            if (producto == null) return BadRequest("Los datos del producto son nulos");
            if (string.IsNullOrEmpty(producto.NombreProducto)) return BadRequest("El nombre es obligatorio");

            try
            {
                var id = await _repository.crear(producto);
                return CreatedAtAction(nameof(Get), new { id = id }, producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                return StatusCode(500, "Error al guardar en la base de datos");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _repository.eliminar(id);

                if (!eliminado)
                {
                    // Si la respuesta es falsa devuelve el mensaje.
                    return NotFound($"No se encontró el producto con ID {id}");
                }

                return Ok(new { mensaje = "Producto eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el producto");
                return StatusCode(500, "Error interno al intentar eliminar");
            }

        }
         [HttpPut]
        public async Task<ActionResult> Put([FromBody] Producto producto)
        {
            if (producto == null) return BadRequest("Los datos del producto son nulos");
            if (string.IsNullOrEmpty(producto.NombreProducto)) return BadRequest("El nombre es obligatorio");

            try
            {
                var actualizado = await _repository.actualizar(producto);
                if (!actualizado)
                {
                    return NotFound($"No se encontró el producto con ID {producto.IdProducto}");
                }

                return Ok(new { mensaje = "Producto actualizado correctamente" });
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                return StatusCode(500, "Error al guardar en la base de datos");
            }
        }
    }
}