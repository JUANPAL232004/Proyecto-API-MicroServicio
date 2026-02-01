using Microsoft.Data.SqlClient;
using apirest.models;
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
    }
}