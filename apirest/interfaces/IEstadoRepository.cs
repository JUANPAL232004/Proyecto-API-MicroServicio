using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apirest.models;

namespace apirest.interfaces
{
    public interface IEstadoRepository
    {   
        Task<IEnumerable<Estado>> ObtenerTodos();
    }
}