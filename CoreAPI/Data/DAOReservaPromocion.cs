// Librerías
using System.Collections.Generic;
using System.Linq;
using Utilitarios;

namespace Data {

    public class DAOReservaPromocion {

        // Variables
        private readonly Mapeo db = new Mapeo();

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para agregar reserva de promociones
            * Fecha de modificación: 16-04-2020
            * Parámetros: UReservaPromocion nuevaPromocion: Objeto con los datos de la promocion que se quiere agregar
            * Retorna: True si la reserva se guardo correctamente - False si ocurrio un error en la ejecución del método
        */
        public bool CrearReserva(UReservaPromocion nuevaPromocion) {

            try {

                using (this.db) {

                    db.ReservaPromocion.Add(nuevaPromocion);
                    db.SaveChanges();
                    return true;
                }

            } catch { return false; }
        }

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para leer las promociones compradas por un usuario
            * Fecha de modificación: 16-04-2020
            * Parámetros: Int id : Identificador del usuario del cual se quieren ver las promociones compradas
            * Retorna: Lista de las promociones compradas por el usurio - Null si ocurrio un error en la ejecución del método
        */
        public IEnumerable<UReservaPromocion> LeerPromocionesUsuario(int id) {

            try {

                using (this.db) {

                    return db.ReservaPromocion
                          .Where(x => x.UsuarioId == id)
                          .OrderBy(x => x.FechaCompra)
                          .ToList();
                }                

            } catch { return null; }
        }
    }
}
