using Microsoft.AspNetCore.Mvc;
using apirest.interfaces;
using apirest.Models;
using Microsoft.AspNetCore.Authorization;

namespace apirest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepository _repository;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(IProductoRepository repository, ILogger<ProductoController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> InformaciónTodos()
        {
            try 
            {
                var productos = await _repository.ObtenerTodos();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                return StatusCode(500, "Ocurrió un error en el servidor.");
            }
        }

        [HttpGet("{id}")] // Falta este endpoint para cumplir con el estándar REST
        public async Task<ActionResult<Producto>> GetById(int id)
        {
            var producto = await _repository.ObtenerPorId(id);
            if (producto == null) return NotFound($"Producto con ID {id} no encontrado");
            return Ok(producto);
        }

        
        
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Producto producto)
        {
            // Eliminado 
            if (!ModelState.IsValid) return BadRequest(ModelState); 
            
            try
            {
                var IdCreado = await _repository.crear(producto);
                producto.IdProducto = IdCreado;
                return CreatedAtAction(nameof(GetById), new { id = IdCreado }, producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                return StatusCode(500, "Error al procesar la solicitud.");
            }
        }

        [HttpPut("{id}")] // Eliminado 
        public async Task<ActionResult> Update(int id, [FromBody] Producto producto)
        {
            if (id != producto.IdProducto) return BadRequest("El ID del cuerpo no coincide con el de la URL");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var actualizado = await _repository.actualizar(producto);
                if (!actualizado) return NotFound();

                return NoContent(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar");
                return StatusCode(500, "Error interno.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _repository.eliminar(id);
                if (!eliminado) return NotFound();

                return Ok(new { mensaje = "Producto eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar");
                return StatusCode(500, "Error interno.");
            }
        }
    }
}
