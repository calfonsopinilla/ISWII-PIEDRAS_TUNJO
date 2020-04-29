using System.Collections.Generic;
using System.Linq;
using Utilitarios;

namespace Data {

    public class DAOComentarioPictograma {

        // Variables
        private readonly Mapeo dataBase = new Mapeo();

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para agregar un comentario de un pictograma
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioPictograma comentario: Objeto con los datos a insertar
         * Retorna: True si la inserción se hizo de forma correcta - False si ocurre un error durante la ejecución del método
         */
        public bool CrearComentario(UComentarioPictograma comentario) {
            
            try {

                using (this.dataBase) {

                    this.dataBase.ComentarioPictograma.Add(comentario);
                    this.dataBase.SaveChanges();
                    return true;
                }

            } catch { return false; }
        }

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para leer todos los comentarios de un pictograma
         * Fecha Creación: 29/04/2020
         * Parámetros: UComentarioPictograma comentario: Objeto con los datos del evento que se desea leer
         * Retorna: Lista de comentarios
         */
        public List<UComentarioPictograma> LeerComentariosId(UComentarioPictograma comentario) {

            using (this.dataBase) {

                return this.dataBase.ComentarioPictograma.Where(
                        x => x.PictogramaId == comentario.PictogramaId
                    ).ToList();
            }
        }
    }
}
