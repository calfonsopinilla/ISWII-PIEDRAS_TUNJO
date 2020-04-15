using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Data
{
    public class DAOPictograma
    {

        private readonly Mapeo db = new Mapeo();

        public IEnumerable<UPictograma> ObtenerTodos()
        {
            return db.Pictograma.ToList();
        }

        public bool Agregar(UPictograma picto)
        {
            try
            {
                var result = db.Pictograma.Add(picto);
                db.SaveChanges();
                return result != null;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool ExistePorNombre(string nombrePic, bool update = false)
        {
            var results = db.Pictograma.Where(x => x.Nombre.ToLower() == nombrePic.ToLower()).ToList();
            return update ? (results.Count > 1) : (results.Count > 0);
        }

        public UPictograma Buscar(int id)
        {
            return db.Pictograma.Find(id);
        }

        public bool Actualizar(UPictograma pictograma, int id)
        {
            try
            {
                db.Entry(pictograma).State = EntityState.Modified;
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
                var picto = db.Pictograma.Find(id);
                var result = db.Pictograma.Remove(picto);
                db.SaveChanges();
                return result != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Existe(int id)
        {
            return db.Pictograma.Any(x => x.Id == id);
        }
    }
}
