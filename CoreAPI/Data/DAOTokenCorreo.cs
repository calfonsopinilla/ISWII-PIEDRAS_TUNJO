using System;
using System.Linq;
using Utilitarios;

namespace Data {

    public class DAOTokenCorreo {

        // Variables
        private UTokenCorreo usuario;
        private Mapeo conexionBD;

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Agregar usuario a la tabla Token Correo
         */
        public void AgregarTokenCorreo(UTokenCorreo token) {

            using (this.conexionBD = new Mapeo()) {

                this.conexionBD.TokenCorreo.Add(token);
                this.conexionBD.SaveChanges();
            }
        }

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Leer un usuario de la tabla Token Correo
         */
        public UTokenCorreo LeerTokenCorreo(string correoElectronico, string numeroDocumento) {

            using (this.conexionBD = new Mapeo()) {

                try {

                    return this.conexionBD.TokenCorreo.Where(x => x.CorreoElectronico.Equals(correoElectronico) || x.NumeroDocumento.Equals(numeroDocumento)).FirstOrDefault();

                } catch (Exception ex) {
                    throw ex;
                }
            }
        }

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Leer un usuario de la tabla Token Correo filtrado por la columna token
         */
        public UTokenCorreo LeerUsuario(string token) {

            using (this.conexionBD = new Mapeo()) {

                try {

                    return this.conexionBD.TokenCorreo.Where(x => x.Token.Equals(token)).FirstOrDefault();

                } catch (Exception ex) {
                    throw ex;
                }
            }
        }

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Borrar usuario de la tabla Token Correo
         */
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

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Validar si el correo electrónico del registro ya existe en la tabla Token Correo
         */
        public bool ExisteCorreo(string correo) {

            using (var db = new Mapeo()) {
                return db.TokenCorreo.Where(x => x.CorreoElectronico == correo).FirstOrDefault() != null;
            }
        }

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Validar si el número de documento del registro ya existe en la tabla Token Correo
         */
        public bool ExisteNumeroDoc(string numeroDoc) {

            using (var db = new Mapeo()) {
                return db.TokenCorreo.Where(x => x.NumeroDocumento == numeroDoc).FirstOrDefault() != null;
            }
        }
    }
}
