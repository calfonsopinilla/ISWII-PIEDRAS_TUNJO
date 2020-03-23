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

            this.auxiliar = new DAOTokenCorreo().LeerTokenCorreo(usuario.CorreoElectronico);

            if (this.auxiliar == null) {
                new LCorreo().EnviarToken(usuario.CorreoElectronico, usuario.Token, "Su link de verificación es: " + "http://localhost:61365/usuario/registro/validar_token?token=" + usuario.Token);
                new DAOTokenCorreo().AgregarTokenCorreo(usuario);
                return true;
            } else
                return false;
        }
    }
}
