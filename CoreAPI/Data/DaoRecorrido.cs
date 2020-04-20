using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data
{
    public class DaoRecorrido
    {
        private readonly Mapeo db = new Mapeo();

        public IEnumerable<URecorrido> ObtenerTodos()
        {
            return db.Recorridos.ToList();
        }
    }
}
