// Librerias
using System.Collections.Generic;
using Utilitarios;
using Data;

namespace Logica {

    public class LReservaPromocion {

        /*
           * Autor: Jhonattan Pulido
           * Descripción: Método que sirve para agregar reserva de promociones
           * Fecha de modificación: 16-04-2020
           * Parámetros: UReservaPromocion nuevaPromocion: Objeto con los datos de la promocion que se quiere agregar
           * Retorna: True si la reserva se guardo correctamente - False si ocurrio un error en la ejecución del método
       */
        public bool CrearReserva(UReservaPromocion nuevaPromocion) {
            return new DAOReservaPromocion().CrearReserva(nuevaPromocion);
        }

        public IEnumerable<UReservaPromocion> ObtenerTodos()
        {
            return new DAOReservaPromocion().ObtenerTodos();
        }


        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para leer las promociones compradas por un usuario
            * Fecha de modificación: 16-04-2020
            * Parámetros: Int id : Identificador del usuario del cual se quieren ver las promociones compradas
            * Retorna: Lista de las promociones compradas por el usurio - Null si ocurrio un error en la ejecución del método
        */
        public IEnumerable<UReservaPromocion> LeerPromocionesUsuario(int id) {
            return new DAOReservaPromocion().LeerPromocionesUsuario(id);
        }
    }
}
