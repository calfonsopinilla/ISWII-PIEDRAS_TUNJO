using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Logica
{
    public class LRol
    {
        public IEnumerable<URol> ObtenerRoles()
        {
            return new DaoRol().ObtenerRoles();
        }

        public URol Buscar(int id)
        {
            return new DaoRol().Buscar(id);
        }
    }
}
