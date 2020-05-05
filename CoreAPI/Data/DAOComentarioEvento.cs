using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data {

    public class DAOComentarioEvento {

        // Variables
        private readonly Mapeo dataBase = new Mapeo();
        private UComentarioEvento comentario;

        /* Métodos */

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para agregar un comentario de un evento
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioEvento comentario: Objeto con los datos a insertar
         * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
         */
        public bool CrearComentario(UComentarioEvento comentario) {
            
            try {

                using (this.dataBase) {

                    this.dataBase.ComentarioEvento.Add(comentario);
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
        public List<UComentarioEvento> LeerComentariosId(UComentarioEvento comentario) {

            using (this.dataBase) {

                return this.dataBase.ComentarioEvento
                    .Include("Usuario")
                    .Where(
                        x => x.EventoId == comentario.EventoId && x.Reportado == false
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
        public UComentarioEvento LeerComentarioUsuario(UComentarioEvento comentario) {

            using (this.dataBase) {

                return this.dataBase.ComentarioEvento
                    .Include("Usuario")
                    .Where(
                        x => x.UsuarioId == comentario.UsuarioId && x.EventoId == comentario.EventoId
                    ).FirstOrDefault();
            }
        }

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para actualizar un comentario de un evento
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioEvento comentario: Objeto con los datos a insertar
         * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
         */
        public bool ActualizarComentario(UComentarioEvento comentario) {

            this.comentario = new UComentarioEvento();

            using (this.dataBase) {

                this.comentario = this.dataBase.ComentarioEvento.Where(
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
         * Descripción: Método que funciona para borrar un comentario de un evento
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioEvento comentario: Objeto con los datos a insertar
         * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
         */
        public bool BorrarComentario(long id) {

            this.comentario = new UComentarioEvento();

            using (this.dataBase) {

                this.comentario = this.dataBase.ComentarioEvento.Where(x => x.Id == id).FirstOrDefault();
                this.dataBase.ComentarioEvento.Remove(this.comentario);
                this.dataBase.SaveChanges();
                return true;
            }            
        }
    }
}
