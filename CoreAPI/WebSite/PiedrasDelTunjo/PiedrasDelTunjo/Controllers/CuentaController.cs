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

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", methods: "*", headers: "*")]
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
        [Route("cuenta/iniciaSesion")]
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
                //if (!actualizar)
                //{ // En caso de que exista un error a la hora de actualizar la base de datos
                //    return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Ha ocurrido un error" });
                //}
                // Se retorna un mensaje satisfactorio y el token JWT
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, token });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("cuenta/userByToken")]
        public HttpResponseMessage GetUserByToken()
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var userClaim = claimsIdentity.FindFirst("usuario"); // or FindAll()
            var user = JsonConvert.DeserializeObject<UUsuario>(userClaim.Value);
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 06/04/2020
            Descripcion: Método que genera el token al usuario
            Parámetros:
            Retorna:
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
    }
}
