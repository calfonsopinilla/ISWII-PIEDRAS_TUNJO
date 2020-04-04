using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
namespace Data
{
    public class DaoPromocion
    {
        private readonly Mapeo db = new Mapeo();
        public IEnumerable<UPromocion> ObtenerPromociones()
        {
            List<UPromocion> promocion = null;
            try
            {
                promocion = db.promocion
                            .OrderBy(x => x.Id)
                            .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return promocion;
        }

        public UPromocion Buscar(int id)
        {
            try
            {
                return db.promocion.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Agregar(UPromocion promocion)
        {
            try
            {
                db.promocion.Add(promocion);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Actualizar(int id, UPromocion promocion)
        {
            try
            {
                db.Entry(promocion).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Existe(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                var promocion = db.promocion.Find(id);
                db.promocion.Remove(promocion);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Existe(int id)
        {
            return db.promocion.Any(x => x.Id == id);
        }
    }
}
