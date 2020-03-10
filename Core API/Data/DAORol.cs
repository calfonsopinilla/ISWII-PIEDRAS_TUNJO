using System.Collections.Generic;
using System.Linq;
using Utilitarios;

namespace Data {

    public class DAORol {

        // Variables
        private DAOMapeo conexionBD;

        /*
            Autor: Jhonattan Pulido
            Fecha modificacion: 8/03/2020
        */
        public List<URol> LeerRoles() {
            
            using (this.conexionBD = new DAOMapeo()) {
                
                return conexionBD.Roles.ToList<URol>();
            }            
        }
    }
}
