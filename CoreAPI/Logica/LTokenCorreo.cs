using Logic;
using Newtonsoft.Json;
using System;
using Utilitarios;
using Data;

namespace Logica {

    public class LTokenCorreo {

        private UUsuario usuario;
        private UTokenCorreo token;
        private UTokenCorreo auxiliar;

        public bool GenerarToken(string datosUsuario, int aplicacionId) {

            this.usuario = new UUsuario();
            this.usuario = JsonConvert.DeserializeObject<UUsuario>(datosUsuario);
            this.usuario.Token = new LEncriptar().Encriptar(datosUsuario);

            this.token = new UTokenCorreo();
            this.token.Token = this.usuario.Token;
            this.token.CorreoElectronico = this.usuario.CorreoElectronico;
            this.token.FechaGeneracion = DateTime.Now;
            this.token.FechaVencimiento = DateTime.Now.AddMinutes(30);
            this.token.AplicacionId = aplicacionId;

            this.auxiliar = new DAOTokenCorreo().LeerTokenCorreo(this.token.CorreoElectronico);

            if (this.auxiliar == null) {
                new LCorreo().EnviarToken(this.usuario.CorreoElectronico, this.usuario.Token, "Su link de verificación es: " + "http://localhost:61365/usuario/registro/validar_token?token=" + this.usuario.Token);
                new DAOTokenCorreo().AgregarTokenCorreo(this.token);
                return true;
            } else
                return false;
        }
    }
}
