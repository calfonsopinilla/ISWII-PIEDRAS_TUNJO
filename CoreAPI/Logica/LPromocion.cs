using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Logica
{
   public class LPromocion
    {
        public IEnumerable<UPromocion> ObtenerPromociones()
        {
            return new DaoPromocion().ObtenerPromociones();
        }

        public UPromocion Buscar(int id)
        {
            return new DaoPromocion().Buscar(id);
        }

        public bool Agregar(UPromocion promocion)
        {
            return new DaoPromocion().Agregar(promocion);
        }

        public bool Actualizar(int id, UPromocion promocion)
        {
            return new DaoPromocion().Actualizar(id, promocion);
        }

        public bool Eliminar(int id)
        {
            return new DaoPromocion().Eliminar(id);
        }
    }
}
