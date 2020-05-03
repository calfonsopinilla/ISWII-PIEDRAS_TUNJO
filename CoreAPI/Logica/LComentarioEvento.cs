using Data;
using Utilitarios;
using System.Collections.Generic;

namespace Logica {

    public class LComentarioEvento {

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para agregar un comentario de un evento
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioEvento comentario: Objeto con los datos a insertar
         * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
         */
        public bool CrearComentario(UComentarioEvento comentario) { return new DAOComentarioEvento().CrearComentario(comentario); }

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para leer todos los comentarios de un evento
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioEvento comentario: Objeto con los datos del evento que se desea leer
         * Retorna: Lista de comentarios
         */
        public List<UComentarioEvento> LeerComentariosId(UComentarioEvento comentario) { return new DAOComentarioEvento().LeerComentariosId(comentario); }

        /*
         * Autor: Jhonattan Pulido
         * Fecha creación: 29/04/2020
         * Descripción: Método que sirve para leer los comentarios de un evento, noticia, pictograma etc de un usuario.
         * Recibe: String table: nombre de la tabla a referenciar - Int objectId: Identificador del objeto del cual se quiere obtener los comentarios - Int userId: Identificador del usuario
         * Retorna: Comentario del usuario
         */
        public UComentarioEvento LeerComentarioUsuario(UComentarioEvento comentario) { return new DAOComentarioEvento().LeerComentarioUsuario(comentario); }

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para actualizar un comentario de un evento
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioEvento comentario: Objeto con los datos a insertar
         * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
         */
        public bool ActualizarComentario(UComentarioEvento comentario) { return new DAOComentarioEvento().ActualizarComentario(comentario); }

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para borrar un comentario de un evento
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioEvento comentario: Objeto con los datos a insertar
         * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
         */
        public bool BorrarComentario(long id) { return new DAOComentarioEvento().BorrarComentario(id); }
    }
}
