using System;
using System.Linq;
using Utilitarios;

namespace Data {

    public class DAOTokenCorreo {

        private UTokenCorreo usuario;
        private Mapeo conexionBD;

        public void AgregarTokenCorreo(UTokenCorreo token) {

            using (this.conexionBD = new Mapeo()) {

                this.conexionBD.TokenCorreo.Add(token);
                this.conexionBD.SaveChanges();
            }            
        }

        public UTokenCorreo LeerTokenCorreo(string correoElectronico, string numeroDocumento) {

            using (this.conexionBD = new Mapeo()) {

                try {

                    return this.conexionBD.TokenCorreo.Where(x => x.CorreoElectronico.Equals(correoElectronico) || x.NumeroDocumento.Equals(numeroDocumento)).FirstOrDefault();

                } catch (Exception ex) {
                    throw ex;
                }
            }
        }

        public UTokenCorreo LeerUsuario(string token) {

            using (this.conexionBD = new Mapeo()) {

                try {

                    return this.conexionBD.TokenCorreo.Where(x => x.Token.Equals(token)).FirstOrDefault();

                } catch (Exception ex) {
                    throw ex;
                }
            }
        }

        public bool BorrarToken(long tokenId) {

            using (this.conexionBD = new Mapeo()) {

                try {

                    this.usuario = this.conexionBD.TokenCorreo.Find(tokenId);
                    this.conexionBD.TokenCorreo.Remove(this.usuario);
                    this.conexionBD.SaveChanges();
                    return true;

                } catch (Exception ex) {
                    throw ex;
                }
            }
        }
    }
}
