using Data;
using System;
using Utilitarios;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Logica {

    /*
        Autor: Jhonattan Alejandro Pulido Arenas
        Fecha creación: 18/03/2020
        Descripción: Clase que sirve para hacer CRUD de cabañas
    */
    public class LCabana {

        // Variables
        private UCabana cabana;

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para añadir una nueva cabaña a la base de datos
            Recibe: String datosCabana - JSON de tipo cabaña con las especificaciones ya validadas
            Retorna: String con mensaje de confirmación o de error
        */
        public string CrearCabana(string datosCabana, List<string> listaUrls) {

            try {

                this.cabana = new UCabana();
                this.cabana = JsonConvert.DeserializeObject<UCabana>(datosCabana);
                this.cabana.ImagenesUrl = new List<string>();
                this.cabana.ImagenesUrl = listaUrls;

                if (new DAOCabana().LeerCabanaNombre(this.cabana.Nombre) == null) {
                    new DAOCabana().CrearCabana(this.cabana);
                    return "Cabaña creada correctamente";
                } else
                    return "ERROR: La cabaña ya existe";

            } catch (Exception ex) {

                throw ex;
            }            
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer una cabaña filtrada por el Id
            Recibe: Integer cabanaId - ID de la cabaña que se desea traer datos
            Retorna: Objeto de tipo cabaña
        */
        public UCabana LeerCabana(int cabanaId) {

            try {
                return new DAOCabana().LeerCabana(cabanaId);
            } catch (Exception ex) {

                throw ex;
            }            
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer una cabaña filtrada por el nombre
            Recibe: String cabanaNombre - Nombre de la cabaña que se desea traer datos
            Retorna: Objeto de tipo cabaña
        */
        public UCabana LeerCabanaNombre(string cabanaNombre) {

            try {
                return new DAOCabana().LeerCabanaNombre(cabanaNombre);
            } catch (Exception ex) {

                throw ex;
            } 
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer las cabañas existentes alojadas en la base de datos
            Recibe: Nada
            Retorna: Lista de cabañas
        */
        public IEnumerable<UCabana> LeerCabanas() {

            try {
                return new DAOCabana().LeerCabanas();
            } catch (Exception ex) {

                throw ex;
            } 
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para borrar una cabaña de la base de datos
            Recibe: Integer cabanaId - El id de la cabaña que se desea eliminar
            Retorna: Booleano true
        */
        public bool BorrarCabana(int cabanaId) {

            try {
                return new DAOCabana().BorrarCabana(cabanaId);
            } catch (Exception ex) {

                throw ex;
            } 
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para actuaizar una cabaña de la base de datos
            Recibe: UCabana cabana - Objeto que contiene los datos que se quieren modificar
            Retorna: Booleano true
        */
        public string ActualizarCabana(string datosCabana, List<string> listaUrls) {

            try {

                this.cabana = new UCabana();
                this.cabana = JsonConvert.DeserializeObject<UCabana>(datosCabana);
                this.cabana.ImagenesUrl = new List<string>();
                this.cabana.ImagenesUrl = listaUrls;

                if (new DAOCabana().LeerCabanaNombre(this.cabana.Nombre) == null) {
                    new DAOCabana().ActualizarCabana(this.cabana);
                    return "Cabaña actualizada correctamente";
                } else
                    return "ERROR: No se pudo actualizar la cabaña";

            } catch (Exception ex) {

                throw ex;
            }
        }
    }
}