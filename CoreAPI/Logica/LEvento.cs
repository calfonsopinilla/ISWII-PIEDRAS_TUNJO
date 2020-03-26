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

        public UEvento Buscar(int id)
        {
            return new DaoEvento().Buscar(id);
        }

        public bool Agregar(UEvento evento)
        {
            return new DaoEvento().Agregar(evento);
        }

        public bool Actualizar(int id, UEvento evento)
        {
            return new DaoEvento().Actualizar(id, evento);
        }

        public bool Eliminar(int id)
        {
            return new DaoEvento().Eliminar(id);
        }

        

    }
}
