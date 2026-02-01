using Microsoft.Data.SqlClient;
using System.Data;
using apirest.Interfaces;
using apirest.Models;

namespace apirest.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly string _connectionString;

        public LoginRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("cadenaConexionSqlServer")!;
        }

        public async Task<Usuario?> ObtenerPorEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "SELECT IdUsuario, nombreUsuario, contraseña, CorreoElectronico FROM [dbo].[Usuario] WHERE CorreoElectronico = @Email";
            
            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email; 

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Usuario
                {
                    IdUsuario = reader.GetInt32(0),
                    NombreUsuario = reader.GetString(1),
                    Password = reader.GetString(2), 
                    CorreoElectronico = reader.GetString(3)
                };
            }
            return null;
        }

        public async Task<bool> Registrar(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "INSERT INTO [dbo].[Usuario] (nombreUsuario, contraseña, CorreoElectronico) VALUES (@Nombre, @Pass, @Email)";
            
            using var command = new SqlCommand(sql, connection);
            // Uso de tipos explícitos para mayor seguridad en la DB
            command.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = usuario.NombreUsuario;
            command.Parameters.Add("@Pass", SqlDbType.NVarChar).Value = usuario.Password;
            command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = usuario.CorreoElectronico;

            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return result > 0;
        }
    }
}