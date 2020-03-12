using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Logica
{
    public class LEvento
    {
        public IEnumerable<UEvento> ObtenerEventos()
        {
            return new DaoEvento().ObtenerEventos();
        }
    }
}
