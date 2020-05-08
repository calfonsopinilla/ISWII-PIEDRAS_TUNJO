using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data
{
    public class DaoRol
    {
        private readonly Mapeo db = new Mapeo();

        public IEnumerable<URol> ObtenerRoles()
        {
            return db.Roles.ToList();
        }

        public URol Buscar(int id)
        {
            return db.Roles.Find(id);
        }
    }
}
