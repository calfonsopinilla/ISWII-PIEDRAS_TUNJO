using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data {

    /*
        Autor: Jhonattan Alejandro Pulido Arenas
        Fecha creación: 24/03/2020
        Descripción: Clase que sirve para calcular puntuaciones
    */
    public class DAOPuntuacion {

        // Variables
        private Mapeo conexionBD;
        private List<UPuntuacion> listaPuntuaciones;

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 24/03/2020
            Descripción: Método que sirve para leer las puntuaciones de un objeto insertado en una tabla
            Recibe: UPuntuacion puntuacion - Objeto con los atributos a guardar
            Retorna: Booleano
        */
        public bool AgregarPuntuacion(UPuntuacion puntuacion) {

            try {

                using (this.conexionBD = new Mapeo()) {

                    this.conexionBD.Puntuacion.Add(puntuacion);
                    this.conexionBD.SaveChanges();
                    return true;
                }

            } catch (Exception ex) {

                throw ex;                   
            }
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 24/03/2020
            Descripción: Método que sirve para leer las puntuaciones de un objeto insertado en una tabla
            Recibe: Integer puntero - Id de la tabla a la que se va a hacer referencia, Int punteroId - Id del objeto al que se le va a apuntar
            Retorna: Lista de puntuaciones
        */
        public List<UPuntuacion> LeerPuntuacionesObjeto(int puntero, int punteroId) {

            try {

                using (this.conexionBD = new Mapeo()) {

                    this.listaPuntuaciones = this.conexionBD.Puntuacion.Where(x => x.Puntero.Equals(puntero) && x.PunteroId.Equals(punteroId)).ToList();
                    return this.listaPuntuaciones;
                }

            } catch (Exception ex) {

                throw ex;
            }  
        }
    }
}
