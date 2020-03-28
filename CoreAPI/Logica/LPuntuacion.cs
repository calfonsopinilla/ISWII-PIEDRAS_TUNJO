using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Logica {

    /*
        Autor: Jhonattan Alejandro Pulido Arenas
        Fecha creación: 24/03/2020
        Descripción: Clase que sirve para calcular puntuaciones
    */
    public class LPuntuacion {

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 24/03/2020
            Descripción: Método que sirve para leer las puntuaciones de un objeto insertado en una tabla
            Recibe: Integer puntero - Id de la tabla a la que se va a hacer referencia, Int punteroId - Id del objeto al que se le va a apuntar
            Retorna: Lista de puntuaciones
        */
        public List<UPuntuacion> LeerPuntuacionesObjeto(int puntero, int punteroId) {

            try {                
                    
                return new DAOPuntuacion().LeerPuntuacionesObjeto(puntero, punteroId);

            } catch (Exception ex) {

                throw ex;
            }  
        }
    }
}
