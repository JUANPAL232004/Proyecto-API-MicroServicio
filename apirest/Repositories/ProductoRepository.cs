using Microsoft.Data.SqlClient;
using apirest.Models;
using apirest.interfaces;

namespace apirest.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly string _connectionString;

        public ProductoRepository(IConfiguration configuration)
        {
            // Se obtiene la cadena desde appsettings.json
            _connectionString = configuration.GetConnectionString("cadenaConexionSqlServer")!;
        }

            //Funcion donde se hace la consulta y se asignan las columnas para ser llamado a interfaces
        public async Task<IEnumerable<Producto>> ObtenerTodos()
        {
            var lista = new List<Producto>();
            // Agregamos el JOIN y la columna e.NombreEstado
            const string query = @"
                SELECT p.IdProducto, p.NombreProducto, p.Cliente, p.Precio, p.Stock, p.IdEstado, p.Fecha, e.NombreEstado 
                FROM [dbo].[Producto] p
                INNER JOIN [dbo].[Estado] e ON p.IdEstado = e.IdEstado";            
            
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Producto
                        {
                            IdProducto = reader.GetInt32(0),
                            NombreProducto = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Cliente = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Precio = Convert.ToDecimal(reader.GetValue(3)), 
                            Stock = reader.GetInt32(4),
                            IdEstado = reader.GetInt32(5),
                            Fecha = reader.GetDateTime(6),
                            NombreEstado = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)});
                    }
                }
            }
            return lista;
        }


        public async Task<Producto?> ObtenerPorId(int id)
        {
            const string query = @"
            SELECT p.IdProducto, p.NombreProducto, p.Cliente, p.Precio, p.Stock, p.IdEstado, p.Fecha, e.NombreEstado 
        FROM [dbo].[Producto] p
            INNER JOIN [dbo].[Estado] e ON p.IdEstado = e.IdEstado";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Producto
                            {
                                IdProducto = reader.GetInt32(0),
                                NombreProducto = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Cliente = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Precio = reader.GetDecimal(3),
                                Stock = reader.GetInt32(4),
                                IdEstado = reader.GetInt32(5),
                                Fecha = reader.GetDateTime(6)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task<int> Crear(Producto producto)
        {   
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                
                //consulta de el procedimiento almacenado para insert de productos
                using (var cmd = new SqlCommand("sp_CrearProducto", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure; 
                    
                    cmd.Parameters.AddWithValue("@Nombre", producto.NombreProducto);
                    cmd.Parameters.AddWithValue("@Cliente", producto.Cliente);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@IdEstado", producto.IdEstado);

                    var resultado = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(resultado);
                }
            }
        }
        public async Task<bool> Eliminar(int id)
        {
            const string query = @"DELETE FROM [dbo].[Producto] WHERE IdProducto = @Id";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    // ExecuteNonQuery devuelve el número de filas eliminadas.
                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    
                    // Si es mayor a 0, significa que sí encontró el producto y borra este dato.
                    return filasAfectadas > 0;
                }
            }
        }
        public async Task<bool> Actualizar(Producto producto)
        {
            const string query = @"UPDATE [dbo].[Producto] SET NombreProducto = @Nombre, 
                               Precio = @Precio, 
                               Stock = @Stock, 
                               IdEstado = @IdEstado
                               WHERE IdProducto = @Id";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", producto.IdProducto);
                    cmd.Parameters.AddWithValue("@Nombre", producto.NombreProducto);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@IdEstado", producto.IdEstado);

                    int filasAfectadas = await cmd.ExecuteNonQueryAsync();
                    return filasAfectadas > 0;
                }
            }
        }
        
    }
}