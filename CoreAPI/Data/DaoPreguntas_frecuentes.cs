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
    public class DaoPreguntas_frecuentes
    {
        private readonly Mapeo db = new Mapeo();
        public IEnumerable<UPreguntas_frecuentes> ObtenerPreguntasFrecuentes()
        {
            List<UPreguntas_frecuentes> preguntas_frecuentes = null;
            try
            {
                preguntas_frecuentes = db.preguntas_Frecuentes
                            .OrderBy(x => x.Id)
                            .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return preguntas_frecuentes;
        }

        public UPreguntas_frecuentes Buscar(int id)
        {
            try
            {
                return db.preguntas_Frecuentes.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Agregar(UPreguntas_frecuentes preguntas_frecuentes)
        {
            try
            {
                db.preguntas_Frecuentes.Add(preguntas_frecuentes);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Actualizar(int id, UPreguntas_frecuentes preguntas_frecuentes)
        {
            try
            {
                db.Entry(preguntas_frecuentes).State = EntityState.Modified;
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
                var preguntas_frecuentes = db.preguntas_Frecuentes.Find(id);
                db.preguntas_Frecuentes.Remove(preguntas_frecuentes);
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
            return db.preguntas_Frecuentes.Any(x => x.Id == id);
        }
    }
}
