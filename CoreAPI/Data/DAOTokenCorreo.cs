using System;
using System.Linq;
using Utilitarios;

namespace Data {

    public class DAOTokenCorreo {

        private Mapeo conexionBD;

        public void AgregarTokenCorreo(UTokenCorreo token) {

            using (this.conexionBD = new Mapeo()) {

                this.conexionBD.TokenCorreo.Add(token);
                this.conexionBD.SaveChanges();
            }            
        }

        public UTokenCorreo LeerTokenCorreo(string correoElectronico) {

            using (this.conexionBD = new Mapeo()) {

                try {

                    return this.conexionBD.TokenCorreo.Where(x => x.CorreoElectronico.Equals(correoElectronico)).FirstOrDefault();

                } catch (Exception ex) {
                    throw ex;
                }
            }
        }
    }
}
