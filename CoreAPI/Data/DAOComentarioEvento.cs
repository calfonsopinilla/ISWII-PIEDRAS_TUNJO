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
                        x => x.EventoId == comentario.EventoId
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
                        x => x.UsuarioId == comentario.UsuarioId
                    ).FirstOrDefault();
            }
        }
    }
}
