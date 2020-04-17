using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Logica {

   public class LPromocion {

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para crear promociones
            * Fecha de modificación: 15-04-2020
            * Parámetros: UPromocion nuevaPromocion - Objeto con los datos de la promoción
            * Retorna: True si la promoción se almaceno correctamente - False si ocurrio un error en la ejecución del método
        */
        public bool CrearPromocion(UPromocion nuevaPromocion) {
            return new DaoPromocion().CrearPromocion(nuevaPromocion);
        }

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para leer todas las promociones
            * Fecha de modificación: 15-04-2020
            * Parámetros: string estado - "1" si se quieren obtener las promociones habilitadas, "2" si se quieren obtener las promociones deshabilitadas
            * Retorna: Lista tipo UPromocion con las promociones - Null si ocurrio un error en la ejecución del método
        */
        public List<UPromocion> LeerPromociones(string estado) {
            return new DaoPromocion().LeerPromociones(estado);
        }

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para cambiar el estado de una promoción
            * Fecha de modificación: 15-04-2020
            * Parámetros: Int id - Identificador de la promoción
            * Retorna: True si el cambio de estado se efectuó correctamente - False si ocurrio un error en la ejecución del método
        */
        public bool CambiarEstado(int id) {
            return new DaoPromocion().CambiarEstado(id);
        }

        /*
            * Autor: Mary Zapata
            * Descripción: Método que sirve para actualizar promociones
            * Fecha de modificación: 15-04-2020
            * Parámetros: UPromocion nuevaPromocion - Objeto con los datos de la promoción, Int id - Identificador de la promoción
            * Retorna: True si la promoción se actualizo correctamente - False si ocurrio un error en la ejecución del método
        */
        public bool Actualizar(int id, UPromocion promocion) {
            return new DaoPromocion().Actualizar(id, promocion);
        }

        /*public IEnumerable<UPromocion> ObtenerPromociones()
        {
            return new DaoPromocion().ObtenerPromociones();
        }*/

        /*public UPromocion Buscar(int id)
        {
            return new DaoPromocion().Buscar(id);
        }*/

        /*public bool Agregar(UPromocion promocion)
        {
            return new DaoPromocion().Agregar(promocion);
        }*/

        /*public bool Eliminar(int id)
        {
            return new DaoPromocion().Eliminar(id);
        }*/
    }
}
