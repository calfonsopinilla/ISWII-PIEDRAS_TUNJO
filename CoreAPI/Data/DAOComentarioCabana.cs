using System.Collections.Generic;
using Utilitarios;
using System.Linq;

namespace Data {

    public class DAOComentarioCabana {

        // Variables
        private readonly Mapeo dataBase = new Mapeo();
        private UComentarioCabana comentario;

        /* Métodos */

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para agregar un comentario de una cabaña
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioCabana comentario: Objeto con los datos a insertar
         * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
         */
        public bool CrearComentario(UComentarioCabana comentario) {
            
            try {

                using (this.dataBase) {

                    this.dataBase.ComentarioCabana.Add(comentario);
                    this.dataBase.SaveChanges();
                    return true;
                }

            } catch { return false; }
        }

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para leer todos los comentarios de un evento
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioEvento comentario: Objeto con los datos del evento que se desea leer
         * Retorna: Lista de comentarios
         */
        public List<UComentarioCabana> LeerComentariosId(UComentarioCabana comentario) {

            using (this.dataBase) {

                return this.dataBase.ComentarioCabana.Where(
                        x => x.CabanaId == comentario.CabanaId
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
        public UComentarioCabana LeerComentarioUsuario(UComentarioCabana comentario) {

            using (this.dataBase) {

                return this.dataBase.ComentarioCabana
                    .Include("Usuario")
                    .Where(
                        x => x.UsuarioId == comentario.UsuarioId
                    ).FirstOrDefault();
            }
        }

        /*
        * Autor: Jhonattan Pulido
        * Descripción: Método que funciona para actualizar un comentario de una cabaña
        * Fecha Creación: 29/04/2020
        * Parámetros: UComentarioCabana comentario: Objeto con los datos a insertar
        * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
        */
        public bool ActualizarComentario(UComentarioCabana comentario) {

            this.comentario = new UComentarioCabana();

            using (this.dataBase) {

                this.comentario = this.dataBase.ComentarioCabana.Where(
                    x => x.Id == comentario.Id).FirstOrDefault();

                if (this.comentario != null) {

                    this.dataBase.Entry(this.comentario).CurrentValues.SetValues(comentario);
                    this.dataBase.SaveChanges();
                    return true;

                } else
                    return false;                                
            }
        }

        /*
        * Autor: Jhonattan Pulido
        * Descripción: Método que funciona para borrar un comentario de una cabaña
        * Fecha Creación: 29/04/2020
        * Parámetros: UComentarioCabana comentario: Objeto con los datos a insertar
        * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
        */
        public bool BorrarComentario(long id) {

            this.comentario = new UComentarioCabana();

            using (this.dataBase) {

                this.comentario = this.dataBase.ComentarioCabana.Find(id);
                this.dataBase.ComentarioCabana.Remove(this.comentario);
                this.dataBase.SaveChanges();
                return true;                    
            }  
        }
    }
}
