using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

// Librerias para el token
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using Logica;
using Utilitarios;
using Newtonsoft.Json;
using System.Configuration;
using System.Web;
using Logic;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    [RoutePrefix("cuenta")]
    public class CuentaController : ApiController
    {

        /*
              @Autor : Jose Luis Soriano Roa
              *Fecha de creación: 18/03/2020
              *Descripcion : Metodo que trae la informacion del usuario que va a iniciar sesion.
              *Este metodo recibe : Resive un objeto de tipo Ulogin quien incorpora correoElectronico y clave en json 
      */
        [HttpPost]
        [AllowAnonymous]
        [Route("iniciaSesion")]
        public HttpResponseMessage IniciarSesion([FromBody] UUsuario usuario){
            try
            {
                var userLogin = new LCuenta().IniciarSesion(usuario.CorreoElectronico, usuario.Clave);
                if (userLogin == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "El usuario no existe" });
                }
                // Se crea el token y se almacena en la variable token
                string token = GenerateToken(userLogin);
                //userLogin.Token = token;
                //// Se actualiza el token de la BD
                //bool actualizar = new LUsuario().Actualizar(userLogin.Id, userLogin);
                //if (!actualizar) { // En caso de que exista un error a la hora de actualizar la base de datos
                //    return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Ha ocurrido un error" });
                //}
                // Se retorna un mensaje satisfactorio y el token JWT
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, token });
            }

            catch (Exception ex) {
                throw ex;
            }
        }

        /*
         Autor: Steven Cruz
         Fecha: 08/04/2020
         Desc: Servicio que regresa el usuario tokenizado
         */
        [HttpGet]
        [Authorize]
        [Route("userByToken")]
        public HttpResponseMessage GetUserByToken()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var userClaim = claimsIdentity.FindFirst("usuario"); // or FindAll()
            var usuario = JsonConvert.DeserializeObject<UUsuario>(userClaim.Value);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, usuario });
        }

        /*
         * Autor: Steven Cruz
         * Fecha: 08/04/2020
         * Desc: Actualizar cuenta del usuario y regresar nuevo token
         */
        [HttpPut]
        [Authorize]
        [Route("update/{id}")]
        public HttpResponseMessage Update([FromUri] int id, [FromBody] UUsuario usuario)
        {
            if (id != usuario.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Bad Request" });
            }
            var updated = new LUsuario().Actualizar(id, usuario);
            if(updated)
            {
                string token = GenerateToken(usuario);
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, token });
            } else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false });
            }
        }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 06/04/2020
            Descripcion: Método que elimina el token del usuario
            Parámetros: Long id : Id del usuario que desea cerrar sesión
            Retorna: Booleano
        */
        [HttpGet]
        [Route("CerrarSesion")]
        public HttpResponseMessage CerrarSesion([FromUri] int id) {
            try {

                UUsuario usuario = new LUsuario().Buscar(id); // Se obtiene el usuario
                usuario.Token = ""; // Se vacia el token                
                bool actualizar = new LUsuario().Actualizar(usuario.Id, usuario); // Se actualiza el token en la bd

                if (actualizar)
                    return Request.CreateResponse(HttpStatusCode.OK, new { ok = true});
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "Ha ocurrido un error" });

            } catch (Exception ex) { throw ex; }
        }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 06/04/2020
            Descripcion: Método que genera el token al usuario
            Parámetros: UUsuario usuario : Contiene los datos del usuario que se loggea
            Retorna: El token generado
        */

        private string GenerateToken(UUsuario usuario) {            
            // Variables de configuración Jwt
            var _secrectKey = ConfigurationManager.AppSettings["SecretKey"];
            var _issuer = ConfigurationManager.AppSettings["Issuer"];
            var _audience = ConfigurationManager.AppSettings["Audience"];
            if (!Int32.TryParse(ConfigurationManager.AppSettings["Expires"], out int _expires))
                _expires = 24;
            // CREAMOS EL HEADER
            var _symmetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(_secrectKey));
            var _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var _header = new JwtHeader(_signingCredentials);
            // CREAMOS LOS CLAIMS
            var _claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("usuario", JsonConvert.SerializeObject(usuario))
            };
            // CREAMOS EL PAYLOAD
            var _payload = new JwtPayload(
                    issuer: _issuer,
                    audience: _audience,
                    claims: _claims,
                    notBefore: DateTime.UtcNow,
                    // expira a las 24 horas
                    expires: DateTime.UtcNow.AddHours(_expires)
                );
            // GENERAMOS EL TOKEN
            var _token = new JwtSecurityToken(_header, _payload);
            return new JwtSecurityTokenHandler().WriteToken(_token);
        }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 07/05/2020
            Descripcion: Método que genera código para recuperar contraseña
            Parámetros: String correoElectronico, String numeroDocumento
            Retorna: True - Si el coódigo se envio satisfactoriamente, False - Si ocurrio un error durante la ejecución del método
            Ruta: .../cuenta/recuperar-clave/generar-codigo?correoElectronico=a@a.com&numeroDocumento=123
        */
        [HttpGet]
        [Route("recuperar-clave/generar-codigo")]
        public HttpResponseMessage GenerarCodigo([FromUri] string correoElectronico, [FromUri] string numeroDocumento) {

            if (correoElectronico != null && numeroDocumento != null) {

                UUsuario usuario = new LCuenta().LeerUsuario(correoElectronico, numeroDocumento);
                URecuperarClave recuperarClave = new URecuperarClave();                

                if (usuario != null) {

                    if (new LCuenta().LeerRecuperarUserId(usuario.Id) == null) {

                        usuario.Token = new LEncriptar().CodigoVerificacion();

                        new LCorreo().EnviarToken(usuario.CorreoElectronico, usuario.Token, usuario.Token);
                        recuperarClave.UserId = usuario.Id;
                        recuperarClave.Token = usuario.Token;
                        recuperarClave.LastModification = DateTime.Now;
                        if (new LCuenta().CrearCodigoRecuperacion(recuperarClave) == true)
                            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Se ha enviado un código de verificación a su correo electrónico" });
                        else
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Ha ocurrido un error inesperado" });
                    } else
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Ya se ha generado un codigo de verificación en su correo" });
                } else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "El correo electronico y/o la contraseña son incorrectos" });

            } else
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "El correo electronico y/o la contraseña son incorrectos" });
        }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 07/05/2020
            Descripcion: Método que actualiza la contraseña de un usuario
            Parámetros: String codigoVerificacion, String clave
            Retorna: True - Si la contraseña se cambia satisfactoriamente, False - Si ocurrio un error durante la ejecución del método
            Ruta: .../cuenta/recuperar-clave/cambiar?codigoVerificacion=*****&clave=123
        */
        [HttpGet]
        [Route("recuperar-clave/cambiar")]
        public HttpResponseMessage CambiarClave([FromUri] string codigoVerificacion, [FromUri] string clave) {

            if (codigoVerificacion != null && clave != null) {

                URecuperarClave recuperarClave = new LCuenta().LeerRecuperarClave(codigoVerificacion);

                if (recuperarClave != null) {

                    UUsuario usuario = new LUsuario().Buscar(recuperarClave.UserId);

                    if (usuario != null) {

                        usuario.Clave = clave;

                        if (new LUsuario().Actualizar(usuario.Id, usuario) == true) {
                            if (new LCuenta().BorrarRecuperarClave(recuperarClave))
                                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Se actualizo la contraseña correctamente" });
                            else
                                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Ha ocurrido un error inesperado" });
                        } else
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Ha ocurrido un error inesperado" });
                    } else
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Ha ocurrido un error inesperado" });
                } else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Ha ocurrido un error inesperado" });
            } else
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Los datos no son correctos" });
        }
    }
}
