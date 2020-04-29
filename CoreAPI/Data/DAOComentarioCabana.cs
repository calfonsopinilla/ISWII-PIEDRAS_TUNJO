using System.Collections.Generic;
using Utilitarios;
using System.Linq;

namespace Data {

    public class DAOComentarioCabana {

        // Variables
        private readonly Mapeo dataBase = new Mapeo();

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
    }
}
