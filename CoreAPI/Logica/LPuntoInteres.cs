using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Logica
{
    public class LPuntoInteres
    {
        public IEnumerable<UPuntoInteres> ObtenerTodos()
        {
            return new DaoPuntoInteres().ObtenerTodos();
        }
    }
}
