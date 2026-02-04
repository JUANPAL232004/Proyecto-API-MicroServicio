using Microsoft.AspNetCore.Mvc;
using apirest.Interfaces; // Interfaz del Servicio
using apirest.DTOs;       // Tus DTOs
using Microsoft.AspNetCore.Authorization;

namespace apirest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _service;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(IProductoService service, ILogger<ProductoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> InformaciónTodos()
        {
            try 
            {
                // Se llama el servicio DTOs
                var productos = await _service.ObtenerTodos();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                return StatusCode(500, "Ocurrió un error en el servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoResponse>> GetById(int id)
        {
            var producto = await _service.ObtenerPorId(id);
            if (producto == null) return NotFound($"Producto con ID {id} no encontrado");
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductoCreate dto)
        {
            try 
            {
                var id = await _service.CrearNuevoProducto(dto);
                return CreatedAtAction(nameof(GetById), new { id = id }, new { id, mensaje = "Creado con éxito" });  
                          }
            catch (ArgumentException ex) 
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ProductoCreate dto) // Usamos DTO
        {
            try
            {
                var actualizado = await _service.Actualizar(id, dto);
                if (!actualizado) return NotFound();

                return Ok(new { mensaje = "Actualizado correctamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
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
                var eliminado = await _service.Eliminar(id);
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