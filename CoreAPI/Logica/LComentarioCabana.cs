using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using Data;

namespace Logica {

    public class LComentarioCabana {

        /*
        * Autor: Jhonattan Pulido
        * Descripción: Método que funciona para agregar un comentario de una cabaña
        * Fecha Creación: 29/04/2020
        * Parámetros: UComentarioCabana comentario: Objeto con los datos a insertar
        * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
        */
        public bool CrearComentario(UComentarioCabana comentario) { return new DAOComentarioCabana().CrearComentario(comentario); }

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para leer todos los comentarios de un evento
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioEvento comentario: Objeto con los datos del evento que se desea leer
         * Retorna: Lista de comentarios
         */
        public List<UComentarioCabana> LeerComentariosId(UComentarioCabana comentario) { return new DAOComentarioCabana().LeerComentariosId(comentario); }
    }
}
