using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data
{
    public class DaoEvento
    {
        private readonly Mapeo db = new Mapeo();

        public IEnumerable<UEvento> ObtenerEventos()
        {
            List<UEvento> eventos = null;
            try
            {
                eventos = db.Eventos
                            .OrderBy(x => x.Id)
                            .ToList();
            } catch(Exception ex)
            {
                throw ex;
            }
            return eventos;
        }
    }
}
