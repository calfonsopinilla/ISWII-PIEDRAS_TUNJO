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

        public UEvento Buscar(int id)
        {
            try
            {
                return db.Eventos.Find(id);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool Agregar(UEvento evento)
        {
            try
            {
                db.Eventos.Add(evento);
                db.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool Actualizar(int id, UEvento evento)
        {
            try
            {
                db.Entry(evento).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            } catch (DbUpdateConcurrencyException)
            {
                if (!Existe(id))
                {
                    return false;
                } else
                {
                    throw;
                }
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                var evento = db.Eventos.Find(id);
                db.Eventos.Remove(evento);
                db.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool Existe(int id)
        {
            return db.Eventos.Any(x => x.Id == id);
        }
    }
}
