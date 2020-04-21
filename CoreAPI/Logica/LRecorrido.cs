using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Logica
{
    public class LRecorrido
    {
        public IEnumerable<URecorrido> ObtenerTodos()
        {
            return new DaoRecorrido().ObtenerTodos();
        }
    }
}
