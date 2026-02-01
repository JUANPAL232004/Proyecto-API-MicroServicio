using Microsoft.Data.SqlClient;
using apirest.Models;
using apirest.interfaces;
using Microsoft.Extensions.Configuration;

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
            const string query = "SELECT IdProducto, NombreProducto, Cliente, Precio, Stock, IdEstado, Fecha FROM [dbo].[Producto]";            
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
                            Precio = Convert.ToDecimal(reader.GetFieldValue<float>(3)), 
                            Stock = reader.GetInt32(4),
                            IdEstado = reader.GetInt32(5),
                            Fecha = reader.GetDateTime(6)
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<bool> crear(Producto producto)
        {
            const string query = @"INSERT INTO [dbo].[Producto] (NombreProducto, Cliente, Precio, Stock, IdEstado, Fecha) 
                           VALUES (@Nombre, @Cliente, @Precio, @Stock, @IdEstado, @Fecha)";
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", producto.NombreProducto);
                    cmd.Parameters.AddWithValue("@Cliente", producto.Cliente);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@IdEstado", producto.IdEstado);
                    cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);

                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
            }
        }
        public async Task<bool> eliminar(int id)
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
        public async Task<bool> actualizar(Producto producto)
        {
            const string query = @"UPDATE [dbo].[Producto] SET SET NombreProducto = @Nombre, 
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