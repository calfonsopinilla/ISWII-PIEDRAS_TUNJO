using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Utilitarios;

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
        public List<UNoticia> informacionNoticia()
        {
            using (var db = new Mapeo())
            {
                try
                {
                    return db.Noticias.OrderBy(x => x.Id).ToList();
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
        public void agregarNoticias(UNoticia noticia)
        {
            using (var db = new Mapeo())
            {
                try
                {
                    db.Noticias.Add(noticia);
                    db.SaveChanges();
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
       *Descripcion: Actualiza la noticia de la noticia que coincida con el id
       *Recibe: Recibe un objeto noticia para actualiza
       *Retorna: 
       */
        public void actulizarNoticia(UNoticia noticia)
        {
            using (var db = new Mapeo())
            {
                UNoticia query = db.Noticias.Where(x => x.Id == noticia.Id).FirstOrDefault();
                query.Titulo = noticia.Titulo;
                query.Descripcion = noticia.Descripcion;
                query.FechaPublicacion = noticia.FechaPublicacion;
                query.ComentariosId = noticia.ComentariosId;
                query.Calificacion = noticia.Calificacion;

                db.Noticias.Attach(query);

                var entry = db.Entry(query);
                entry.State = EntityState.Modified;

                db.SaveChanges();
            }
        }
        /*
       @Autor: Carlos Alfonso Pinilla Garzon
       *Fecha de creación: 18/03/2020
       *Descripcion: Elimina noticias por medio del id que coincida
       *Recibe: un entero id para eliminar la noticia que coincida 
       *Retorna: 
       */
        public void eliminarNoticia(int id)
        {
            using (var db = new Mapeo())
            {
                UNoticia noticia = db.Noticias.Where(x => x.Id == id).FirstOrDefault();

                db.Noticias.Attach(noticia);

                var entry = db.Entry(noticia);
                entry.State = EntityState.Deleted;
                db.SaveChanges();
            }

        }
        /*
       @Autor: Carlos Alfonso Pinilla Garzon
       *Fecha de creación: 18/03/2020
       *Descripcion: Busca el id de la noticia
       *Recibe: un entero id para buscar la noticia que coincida
       *Retorna: true para saber que existe, y false para el caso contrario
       */
        public bool buscarId(int id)
        {
            using (var db = new Mapeo())
            {
                try
                {
                    if (db.Noticias.Where(x => x.Id.Equals(id)).ToList().Count() > 0)
                    {
                        return true;//Existe la noticia
                    }
                    else
                    {
                        return false;//No existe
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
