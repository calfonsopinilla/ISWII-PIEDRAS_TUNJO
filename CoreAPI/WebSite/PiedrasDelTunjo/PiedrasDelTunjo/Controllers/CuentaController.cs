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

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    public class CuentaController : ApiController
    {

        // Variables
        private string token = null;

        /*
              @Autor : Jose Luis Soriano Roa
              *Fecha de creación: 18/03/2020
              *Descripcion : Metodo que trae la informacion del usuario que va a iniciar sesion.
              *Este metodo recibe : Resive un objeto de tipo Ulogin quien incorpora correoElectronico y clave en json 
      */
        [HttpPost]
        [Route("cuenta/iniciaSesion")]
        public HttpResponseMessage IniciarSesion([FromBody] UUsuario usuario){
            try
            {
                var userLogin = new LCuenta().IniciarSesion(usuario.CorreoElectronico, usuario.Clave);

                if (userLogin == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "El usuario no existe" });
                }

                // Fecha de creación del token
                DateTime issuedAt = DateTime.Now;
                DateTime expires = issuedAt.AddMonths(1);
                
                // Se crea el token y se almacena en la variable token
                userLogin.Token = this.BuildToken(userLogin, issuedAt, expires);

                // Se actualiza el token de la BD
                bool actualizar = new LUsuario().Actualizar(userLogin.Id, userLogin);

                if (!actualizar)
                { // En caso de que exista un error a la hora de actualizar la base de datos
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Ha ocurrido un error" });
                }

                // Se retorna un mensaje satisfactorio y el token JWT
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, token = userLogin.Token });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 06/04/2020
            Descripcion: Método que genera el token al usuario
            Parámetros:
            Retorna:
        */
        private string BuildToken(UUsuario usuario, DateTime issuedAt, DateTime expires) {            

            var tokenHandler = new JwtSecurityTokenHandler();

            var claimsIdentity = new ClaimsIdentity(new[] {
                new Claim("usuario", JsonConvert.SerializeObject(usuario))
                //new Claim("Id", usuario.Id.ToString()), // Id
                //new Claim("Nombre", usuario.Nombre), // Nombre
                //new Claim("Apellido", usuario.Apellido), // Apellido
                //new Claim("CorreoElectronico", usuario.CorreoElectronico), // Correo Electronico
                //new Claim("TipoDocumento", usuario.TipoDocumento), // Tipo Documento
                //new Claim("NumeroDocumento", usuario.NumeroDocumento), // Numero Documento
                //new Claim("LugarExpedicion", usuario.LugarExpedicion), // Lugar de expedicion 
                //new Claim("Icono_url", usuario.Icono_url), // Icono Url
                //new Claim("VerificacionCuenta", usuario.VerificacionCuenta.ToString()), // VerificacionCuenta
                //new Claim("EstadoCuenta", usuario.EstadoCuenta.ToString()), // Estado Cuenta
                //new Claim("RolId", usuario.RolId.ToString()), // Rol id
                //new Claim("FechaNacimiento", usuario.FechaNacimiento.ToString()), // FechaNacimiento                
            });

            // Clave secreta
            const string secrectKey = "[RV#tu*zKJV/v4iR5*=*N#MKX;vPNL&RB_7==C?ZXTZ{k6TRiXztjLc-u?V5VdTMUK6y{&.GSFKU.{?,Tq2F$xg?RXd3;y%g&VvS";
            // Convirtiendo la clave secreta a base64
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secrectKey));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token =
                (JwtSecurityToken)
                tokenHandler.CreateJwtSecurityToken(
                    issuer: "http://www.piedrasdeltunjo.tk/",
                    audience: "http://www.piedrasdeltunjo.tk/",
                    subject: claimsIdentity,
                    notBefore: issuedAt,
                    expires: expires,
                    signingCredentials: signingCredentials);

            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
