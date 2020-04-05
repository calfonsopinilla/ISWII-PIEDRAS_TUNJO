using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilitarios;
using Logica;
using Newtonsoft.Json;
using System.Web.Http.Cors;
using System.IO;
using System;

namespace PiedrasDelTunjo.Controllers {

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TokenCorreoElectronicoController : ApiController {

        // Variables
        private UUsuario usuario;
        private UTokenCorreo usuarioToken;

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 11/03/2020
            Descripción: Método que funciona para generar el token para validar el correo electronico del nuevo usuario que se desea registrar
            Recibe: String datosJson - Datos del nuevo usuario, Int aplicacionId - Id para identificar desde donde se esta solicitando el servicio
            Retorna: Boolean
        */
        [HttpPost]
        [Route("usuario/registro/generar_token")]
        public HttpResponseMessage EnviarTokenCorreo([FromBody] UTokenCorreo usuario) {
            
            var item =  new LTokenCorreo().GenerarToken(usuario);
            if (item)
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "REGISTRO SATISFACTORIO: Se generó un código de activación, revise su correo electrónico para continuar" });
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "ERROR: Ya se genero un código para el correo electronico y/o número de documento ya registrado" });
        }

        [HttpGet]
        [Route("usuario/registro/validar_token")]
        public HttpResponseMessage ValidarTokenCorreo([FromUri] string token) {

            if (!token.Equals(null)) {

                this.usuarioToken = new LTokenCorreo().LeerUsuario(token);

                this.usuario = new UUsuario();
                this.usuario.Nombre = this.usuarioToken.Nombre;
                this.usuario.Apellido = this.usuarioToken.Apellido;
                this.usuario.FechaNacimiento = this.usuarioToken.FechaNacimiento;
                this.usuario.TipoDocumento = this.usuarioToken.TipoDocumento;
                this.usuario.NumeroDocumento = this.usuarioToken.NumeroDocumento;
                this.usuario.LugarExpedicion = null;
                this.usuario.CorreoElectronico = this.usuarioToken.CorreoElectronico;
                this.usuario.Clave = this.usuarioToken.Clave;
                this.usuario.Icono_url = this.usuarioToken.IconoUrl;
                this.usuario.VerificacionCuenta = false;
                this.usuario.EstadoCuenta = true;
                this.usuario.RolId = this.usuarioToken.RolId;
                this.usuario.Imagen_documento = null;
                this.usuario.Token = this.usuarioToken.Token;                

                if (new LCuenta().Registrar(this.usuario)) {
                    new LTokenCorreo().BorrarToken(this.usuarioToken.Id);
                    return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "VALIDACION EXITOSA: Ahora puede iniciar sesión" });
                }                    
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "ERROR: Ha ocurrido un error con el Servidor" });
            } else
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "ERROR: URL Inválida, revise su correo nuevamente" });
        }

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Validar si el correo electrónico del registro ya existe en la tabla Token Correo
         */
        [HttpGet]
        [Route("usuario/registro/existeCorreo")]
        public HttpResponseMessage ExisteCorreo([FromUri] string correo) {

            bool existe = new LTokenCorreo().ExisteCorreo(correo);
            return Request.CreateResponse(HttpStatusCode.OK, new { existe });
        }

        /*
         * Autor: Jhonattan Alejandro Pulido Arenas
         * Fecha: 02/04/2020
         * Descripcion: Validar si el número de documento del registro ya existe en la tabla Token Correo
         */
        [HttpGet]
        [Route("usuario/registro/existeNumeroDoc")]
        public HttpResponseMessage ExisteNumeroDoc([FromUri] string numeroDoc) {

            bool existe = new LTokenCorreo().ExisteNumeroDoc(numeroDoc);
            return Request.CreateResponse(HttpStatusCode.OK, new { existe });
        }
    }
}
