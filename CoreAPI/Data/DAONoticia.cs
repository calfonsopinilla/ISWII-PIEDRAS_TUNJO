using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Utilitarios;
using System.Data.Entity.Infrastructure;

namespace Data
{
    public class DAONoticia
    {
        /*
       @Autor: Carlos Alfonso Pinilla Garzon
       *Fecha de creación: 18/03/2020
       *Descripcion: Servicio para mostrar las noticias
       *Recibe: 
       *Retorna: retorna una lista de objetos noticia 
       */
        public List<UNoticia> ObtenerNoticias()
        {
            using (var db = new Mapeo())
            {
                try
                {
                   // return db.Noticias.OrderBy(x => x.Id).ToList();
                    return db.Noticias.OrderByDescending(x => x.Id).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /*
      @Autor: Carlos Alfonso Pinilla Garzon
      *Fecha de creación: 18/03/2020
      *Descripcion: Agrega las noticias
      *Recibe: Un objeto noticia para agregar
      *Retorna: 
      */
        public bool AgregarNoticia(UNoticia noticia)
        {
            using (var db = new Mapeo())
            {
                try
                {
                    db.Noticias.Add(noticia);
                    db.SaveChanges();
                    //crear notificaciones
                    DaoNotificacion notificaciones = new DaoNotificacion();
                    notificaciones.GenerarNotificaciones("Nueva noticia", "Noticia", noticia.Titulo);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public UNoticia Buscar(int id)
        {

            using (var db = new Mapeo())
            {
                try
                {
                    return db.Noticias.Find(id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool Actualizar(int id, UNoticia noticia)
        {
            using (var db = new Mapeo())
            {
                try
                {
                    db.Entry(noticia).State = EntityState.Modified;
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

        }

        public bool Existe(int id)
        {
            using (var db = new Mapeo())
            {
                return db.Noticias.Any(x => x.Id == id);
            }
        }

     
        /*
       @Autor: Carlos Alfonso Pinilla Garzon
       *Fecha de creación: 18/03/2020
       *Descripcion: Elimina noticias por medio del id que coincida
       *Recibe: un entero id para eliminar la noticia que coincida 
       *Retorna: 
       */
        public bool EliminarNoticia(int id)
        {
            using (var db = new Mapeo())
            {
                UNoticia noticia = db.Noticias.Find(id);
                db.Noticias.Remove(noticia);
                db.SaveChanges();
                return true;
            }

        }
    }
}
