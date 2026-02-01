using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apirest.Models;

namespace apirest.Interfaces
{
    public interface ILoginRepository
{
    Task<Usuario?> ObtenerPorEmail(string email);
    Task<bool> Registrar(Usuario usuario);
}
}