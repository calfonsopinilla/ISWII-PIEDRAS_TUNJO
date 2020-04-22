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

        public URecorrido Buscar(int id)
        {
            return new DaoRecorrido().Buscar(id);
        }

        public bool Agregar(URecorrido recorrido)
        {
            return new DaoRecorrido().Agregar(recorrido);
        }

        public bool Eliminar(int id)
        {
            return new DaoRecorrido().Eliminar(id);
        }
    }
}
