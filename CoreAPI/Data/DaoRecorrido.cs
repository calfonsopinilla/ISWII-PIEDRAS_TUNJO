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

        public URecorrido Buscar(int id)
        {
            return db.Recorridos.Find(id);
        }

        public bool Agregar(URecorrido recorrido)
        {
            try
            {
                var created = db.Recorridos.Add(recorrido);
                db.SaveChanges();
                return created != null;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                var recorrido = db.Recorridos.Find(id);
                db.Recorridos.Remove(recorrido);
                db.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
