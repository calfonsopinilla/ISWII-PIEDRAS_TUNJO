using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Newtonsoft.Json;
using Utilitarios;
namespace Logica
{
    public class LComentarioNoticias
    {
        /*
        * Autor: Jhonattan Pulido
        * Descripción: Método que funciona para agregar un comentario de una noticia
        * Fecha Creación: 29/04/2020
        * Parámetros: UComentarioNoticia comentario: Objeto con los datos a insertar
        * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
        */
        public bool CrearComentario(UComentarioNoticia comentario) { return new DaoComentariosNoticias().CrearComentario(comentario); }

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para leer todos los comentarios de unna noticia
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioEvento comentario: Objeto con los datos del evento que se desea leer
         * Retorna: Lista de comentarios
         */
        public List<UComentarioNoticia> LeerComentariosId(UComentarioNoticia comentario) { return new DaoComentariosNoticias().LeerComentariosId(comentario); }

        /*
         * Autor: Jhonattan Pulido
         * Fecha creación: 29/04/2020
         * Descripción: Método que sirve para leer los comentarios de un evento, noticia, pictograma etc de un usuario.
         * Recibe: String table: nombre de la tabla a referenciar - Int objectId: Identificador del objeto del cual se quiere obtener los comentarios - Int userId: Identificador del usuario
         * Retorna: Comentario del usuario
         */
        public UComentarioNoticia LeerComentarioUsuario(UComentarioNoticia comentario) { return new DaoComentariosNoticias().LeerComentarioUsuario(comentario); }

        /*
        * Autor: Jhonattan Pulido
        * Descripción: Método que funciona para actualizar un comentario de una noticia
        * Fecha Creación: 29/04/2020
        * Parámetros: UComentarioNoticia comentario: Objeto con los datos a insertar
        * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
        */
        public bool ActualizarComentario(UComentarioNoticia comentario) { return new DaoComentariosNoticias().ActualizarComentario(comentario); }

        /*
        * Autor: Jhonattan Pulido
        * Descripción: Método que funciona para borrar un comentario de una noticia
        * Fecha Creación: 29/04/2020
        * Parámetros: UComentarioNoticia comentario: Objeto con los datos a insertar
        * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
        */
        public bool BorrarComentario(long id) { return new DaoComentariosNoticias().BorrarComentario(id); }

        public bool agregarComentarioNoticia(UComentarioNoticia comentarioNotica) {
            return new DaoComentariosNoticias().agregarComentarioNoticia(comentarioNotica);
        }
       
        public IEnumerable<UComentarioNoticia> listaComentariosNoticia(int noticiaId)
        {
            return new DaoComentariosNoticias().ListaComentariosNoticia(noticiaId);
        }


        /// noticias + comentarios
   

        public IEnumerable<UNoticia> enviarNoticiaComentarios() {
            List<UNoticia> noticias = new DaoComentariosNoticias().enviarNoticias();
            if (noticias.Count() < 0)
            {
                return null;
            }
            else {
                for (int i = 0; i < noticias.Count; i++){
                    if (!String.IsNullOrEmpty(noticias[i].ImagenesUrl)) {

                        noticias[i].ListaImagenes = JsonConvert.DeserializeObject<List<string>>(noticias[i].ImagenesUrl);
                    }
                    List<UComentarioNoticia> listaComentariosNoticia = new DaoComentariosNoticias().ListaComentariosNoticia(noticias[i].Id).OrderByDescending(x => x.FechaPublicacion).ToList();
                    noticias[i].ListaNoticias = listaComentariosNoticia;
                }
            }
            return noticias.OrderByDescending(x => x.FechaPublicacion).ToList() ;
        }




        public UNoticia enviarVerNoticia(int id)
        {
            UNoticia noticias = new DaoComentariosNoticias().enviarNoticias().Where(x => x.Id == id ).FirstOrDefault();
            if (noticias==null){
                return null;
            }
            else{                
                    if (!String.IsNullOrEmpty(noticias.ImagenesUrl)){
                        noticias.ListaImagenes = JsonConvert.DeserializeObject<List<string>>(noticias.ImagenesUrl);
                    }
                    List<UComentarioNoticia> listaComentariosNoticia = new DaoComentariosNoticias().ListaComentariosNoticia(noticias.Id);
                    noticias.ListaNoticias = listaComentariosNoticia;
            }
            return noticias;
        }

        public bool reportarComentarioNoticia(long idComentario)
        {
            return new DaoComentariosNoticias().reportarComentarioNotica(idComentario);
        }

        public bool eliminarComentarioNoticia(long idComentario)
        {
            return new DaoComentariosNoticias().eliminarComentarioNoticia(idComentario);
        }


    }
}
