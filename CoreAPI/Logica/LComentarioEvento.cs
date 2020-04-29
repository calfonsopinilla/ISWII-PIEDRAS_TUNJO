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
    }
}
