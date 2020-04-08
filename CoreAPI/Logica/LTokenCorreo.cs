using Logic;
using Newtonsoft.Json;
using System;
using Utilitarios;
using Data;

namespace Logica {

    public class LTokenCorreo {
           
        // Variables
        //private UTokenCorreo auxiliar;

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Guardar el usuario que se desea registrar en la app movil en la tabla Token Correo
         */
        public bool GenerarToken(UTokenCorreo usuario) {

            //usuario.Token = new LEncriptar().Encriptar(JsonConvert.SerializeObject(usuario));                                    
            usuario.Token = new LEncriptar().CodigoVerificacion();
            usuario.FechaGeneracion = DateTime.Now;
            usuario.FechaVencimiento = DateTime.Now.AddMinutes(30);            

            //this.auxiliar = new DAOTokenCorreo().LeerTokenCorreo(usuario.CorreoElectronico, usuario.NumeroDocumento);            

            //if (this.auxiliar == null) {
                new LCorreo().EnviarToken(usuario.CorreoElectronico, usuario.Token, usuario.Token);
                new DAOTokenCorreo().AgregarTokenCorreo(usuario);
                return true;
            //} else
                //return false;
        }

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Guardar el usuario que se desea registrar en la app movil en la tabla Token Correo
         */
        public UTokenCorreo LeerUsuario(string token) {
            return new DAOTokenCorreo().LeerUsuario(token);
        }

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Guardar el usuario que se desea registrar en la app movil en la tabla Token Correo
         */
        public bool BorrarToken(long tokenId) {
            return new DAOTokenCorreo().BorrarToken(tokenId);
        }

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Validar si el correo electrónico del registro ya existe en la tabla Token Correo
         */
        public bool ExisteCorreo(string correo) {

            return new DAOTokenCorreo().ExisteCorreo(correo);
        }

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Validar si el número de documento del registro ya existe en la tabla Token Correo
         */
        public bool ExisteNumeroDoc(string numeroDoc) {

            return new DAOTokenCorreo().ExisteNumeroDoc(numeroDoc);
        }
    }
}
