using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Utilitarios;
namespace Data
{
    public class DaoComentariosNoticias
    {

        private readonly Mapeo db = new Mapeo();
        private UComentarioNoticia comentario;

        /* Métodos */

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para agregar un comentario de una noticia
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioNoticia comentario: Objeto con los datos a insertar
         * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
         */
        public bool CrearComentario(UComentarioNoticia comentario) {
            
            try {

                using (this.db) {

                    this.db.ComentariosNoticias.Add(comentario);
                    this.db.SaveChanges();
                    return true;
                }

            } catch { return false; }
        }

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para leer todos los comentarios de unna noticia
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioEvento comentario: Objeto con los datos del evento que se desea leer
         * Retorna: Lista de comentarios
         */
        public List<UComentarioNoticia> LeerComentariosId(UComentarioNoticia comentario) {            

            using (this.db) {

                return this.db.ComentariosNoticias
                    .Include("Usuario")
                    .Where(
                        x => x.Noticia_id == comentario.Noticia_id
                    ).ToList();
            }
        }

        /*
         * Autor: Jhonattan Pulido
         * Fecha creación: 29/04/2020
         * Descripción: Método que sirve para leer los comentarios de un evento, noticia, pictograma etc de un usuario.
         * Recibe: String table: nombre de la tabla a referenciar - Int objectId: Identificador del objeto del cual se quiere obtener los comentarios - Int userId: Identificador del usuario
         * Retorna: Comentario del usuario
         */
        public UComentarioNoticia LeerComentarioUsuario(UComentarioNoticia comentario) {

            using (this.db) {

                return this.db.ComentariosNoticias
                    .Include("Usuario")
                    .Where(
                        x => x.UsuarioId == comentario.UsuarioId
                    ).FirstOrDefault();
            }
        }

        /*
        * Autor: Jhonattan Pulido
        * Descripción: Método que funciona para actualizar un comentario de una noticia
        * Fecha Creación: 29/04/2020
        * Parámetros: UComentarioNoticia comentario: Objeto con los datos a insertar
        * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
        */
        public bool ActualizarComentario(UComentarioNoticia comentario) {

            this.comentario = new UComentarioNoticia();

            using (this.db) {

                this.comentario = this.db.ComentariosNoticias.Where(
                    x => x.Id == comentario.Id).FirstOrDefault();

                if (this.comentario != null) {

                    this.db.Entry(this.comentario).CurrentValues.SetValues(comentario);
                    this.db.SaveChanges();
                    return true;

                } else
                    return false;                                
            }
        }

        /*
        * Autor: Jhonattan Pulido
        * Descripción: Método que funciona para borrar un comentario de una noticia
        * Fecha Creación: 29/04/2020
        * Parámetros: UComentarioNoticia comentario: Objeto con los datos a insertar
        * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
        */
        public bool BorrarComentario(long id) {

            this.comentario = new UComentarioNoticia();

            using (this.db) {

                this.comentario = this.db.ComentariosNoticias.Find(id);
                this.db.ComentariosNoticias.Remove(this.comentario);
                this.db.SaveChanges();
                return true;
            }
        }

        public IEnumerable<UComentarioNoticia> ListaComentariosAdministrador()
        {
            return db.ComentariosNoticias.ToList();
        }

        public List<UComentarioNoticia> ListaComentariosNoticia(int noticiaId)
        {
            try
            {

                List<UComentarioNoticia> listaComentarios = new List<UComentarioNoticia>();
                var lista = (from comentarios in db.ComentariosNoticias
                             join usuarios in db.Usuarios on comentarios.UsuarioId equals usuarios.Id
                             select new {
                                 id = comentarios.Id,
                                 fechaPublicacion = comentarios.FechaPublicacion,
                                 descripcion = comentarios.Descripcion,
                                 nombreUsuario = usuarios.Nombre + " " + usuarios.Apellido,
                                 calificacion = comentarios.Calificacion,
                                 noticiaId = comentarios.Noticia_id,
                                 reportado = comentarios.Reportado,
                                 idUsuario = comentarios.UsuarioId
                             }).ToList();

                listaComentarios = lista.AsEnumerable().Select(p => new UComentarioNoticia()
                {
                    Id = p.id,
                    FechaPublicacion= p.fechaPublicacion,
                    Descripcion = p.descripcion,
                    NombreUsuario = p.nombreUsuario,
                    Calificacion = p.calificacion,
                    Noticia_id = p.noticiaId,
                    Reportado = p.reportado,
                    UsuarioId =p.idUsuario

                }).ToList();
                              

                return listaComentarios.Where(x => x.Noticia_id == noticiaId && x.Reportado== false).ToList();
   
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<UNoticia> enviarNoticias() {

            try { return db.Noticias.ToList(); } catch(Exception ex) {
                throw ex; 
            }
            
        }



        public bool agregarComentarioNoticia(UComentarioNoticia comentarioNoticia) {
            try {
                db.ComentariosNoticias.Add(comentarioNoticia);
                db.SaveChanges();
                return true;

            } catch (Exception ex) { throw ex; }
        }


        public bool eliminarComentarioNoticia(long idComentario)
        {
            try
            {
                var comentario = db.ComentariosNoticias.Find(idComentario);
                db.ComentariosNoticias.Remove(comentario);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool reportarComentarioNotica(long idComentario)
        {
            try
            {
                var comentario = db.ComentariosNoticias.Find(idComentario);
                comentario.Reportado = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;

            }
        }



    }
}
