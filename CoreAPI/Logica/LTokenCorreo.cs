using Logic;
using Newtonsoft.Json;
using System;
using Utilitarios;
using Data;

namespace Logica {

    public class LTokenCorreo {
                
        private UTokenCorreo auxiliar;
        //private bool correoElectronico;
        //private bool numeroDocumento;

        public bool GenerarToken(UTokenCorreo usuario) {
            
            usuario.Token = new LEncriptar().Encriptar(JsonConvert.SerializeObject(usuario));                                    
            usuario.FechaGeneracion = DateTime.Now;
            usuario.FechaVencimiento = DateTime.Now.AddMinutes(30);            

            this.auxiliar = new DAOTokenCorreo().LeerTokenCorreo(usuario.CorreoElectronico, usuario.NumeroDocumento);
            //this.correoElectronico = new DaoUsuario().buscarCorreo(usuario.CorreoElectronico);
            //this.numeroDocumento = new DaoUsuario().buscarCedula(usuario.NumeroDocumento);

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
    }
}
