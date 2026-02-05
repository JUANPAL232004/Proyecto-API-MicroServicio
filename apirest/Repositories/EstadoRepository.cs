using Microsoft.Data.SqlClient;
using apirest.Models;
using apirest.interfaces;
using Microsoft.Extensions.Configuration;

namespace apirest.Repositories
{
    public class EstadoRepository : IEstadoRepository
    {
        private readonly string _connectionString;

        public EstadoRepository(IConfiguration configuration)
        {
            // Se obtiene la cadena desde appsettings.json
            _connectionString = configuration.GetConnectionString("cadenaConexionSqlServer")!;
        }

            //Funcion donde se hace la consulta y se asignan las columnas para ser llamado a interfaces
        public async Task<IEnumerable<Estado>> ObtenerTodos()
        {
            var lista = new List<Estado>();
            const string query = "SELECT IdEstado, NombreEstado FROM [dbo].[Estado]";            
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Estado
                        {
                            IdEstado = reader.GetInt32(0),
                            NombreEstado = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        });
                    }
                }
            }
            return lista;
        }
    }
}