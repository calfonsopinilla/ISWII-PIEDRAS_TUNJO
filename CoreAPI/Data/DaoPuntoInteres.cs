using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data
{
    public class DaoPuntoInteres
    {
        private readonly Mapeo db = new Mapeo();

        public IEnumerable<UPuntoInteres> ObtenerTodos()
        {
            try
            {
                return db.PuntosInteres.ToList();
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
