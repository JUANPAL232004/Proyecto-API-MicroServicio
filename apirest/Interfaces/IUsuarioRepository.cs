using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apirest.Models;

namespace apirest.interfaces
{
    public interface IUsuarioRepository
    {   
        Task<IEnumerable<Usuario>> ObtenerTodos();
    }
}