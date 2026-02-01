using Microsoft.Data.SqlClient;
using apirest.models;
using apirest.interfaces;
using Microsoft.Extensions.Configuration;

namespace apirest.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            // Se obtiene la cadena desde appsettings.json
            _connectionString = configuration.GetConnectionString("cadenaConexionSqlServer")!;
        }

            //Funcion donde se hace la consulta y se asignan las columnas para ser llamado a interfaces
        public async Task<IEnumerable<Usuario>> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            const string query = "SELECT IdUsuario, nombreUsuario, contraseña, CorreoElectronico FROM [dbo].[Usuario]";            
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new Usuario
                        {
                            IdUsuario = reader.GetInt32(0),
                            NombreUsuario = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Password = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            CorreoElectronico = reader.IsDBNull(3) ? string.Empty : reader.GetString(3)
                        });
                    }
                }
            }
            return lista;
        }
    }
}