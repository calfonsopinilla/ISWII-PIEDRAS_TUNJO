using Logic;
using Newtonsoft.Json;
using System;
using Utilitarios;
using Data;

namespace Logica {

    public class LTokenCorreo {
                
        private UTokenCorreo auxiliar;        

        public bool GenerarToken(UTokenCorreo usuario) {
            
            usuario.Token = new LEncriptar().Encriptar(JsonConvert.SerializeObject(usuario));                                    
            usuario.FechaGeneracion = DateTime.Now;
            usuario.FechaVencimiento = DateTime.Now.AddMinutes(30);            

            this.auxiliar = new DAOTokenCorreo().LeerTokenCorreo(usuario.CorreoElectronico, usuario.NumeroDocumento);            

            if (this.auxiliar == null) {
                new LCorreo().EnviarToken(usuario.CorreoElectronico, usuario.Token, "Su link de verificación es: " + "http://piedrasdeltunjo.tk/usuario/registro/validar_token?token=" + usuario.Token);
                new DAOTokenCorreo().AgregarTokenCorreo(usuario);
                return true;
            } else
                return false;
        }

        public UTokenCorreo LeerUsuario(string token) {
            return new DAOTokenCorreo().LeerUsuario(token);
        }

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
