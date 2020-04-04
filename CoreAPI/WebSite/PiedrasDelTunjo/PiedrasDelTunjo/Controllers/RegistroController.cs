using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilitarios;
using Logica;
using Newtonsoft.Json;
using System.Web.Http.Cors;
using System.Web;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RegistroController : ApiController
    {
        [HttpGet]
        [Route("Registro/Val_EmailYCC")]       
        /**
         * Autor: Gabriel Zapata
         * Desc: Metodo para la validacion de la existencia del correo electronico y la cedula
         * en la bd
         * Return: boolean resultadoVal
         */
        //
        public Boolean Val_EmailYCC([FromUri] string valCorreo, [FromUri] string valDocumento)
        {

            URegistroUser validacion = new URegistroUser();
            int contValidacion = 0;
            string json_EmailYCC;
            Boolean resultadoVal = false;

            if (valCorreo.ToString() == null || valCorreo.ToString() == "")
            {
                validacion.CorreoElectronico = " ";
                contValidacion++;
            }
            if (valDocumento.ToString() == null || valDocumento.ToString() == "")
            {
                validacion.NumDocumento = "";
                contValidacion++;
            }

            if (contValidacion == 0)
            {
                validacion.CorreoElectronico = valCorreo;
                validacion.NumDocumento = valDocumento;
                //validacion.NumDocumento = Convert.ToDouble(valDocumento);
                json_EmailYCC = JsonConvert.SerializeObject(validacion);
            }
            else
            {
                json_EmailYCC = JsonConvert.SerializeObject(validacion);
            }

            validacion = new LRegistro().Validacion_ExistenciaCorreoYCC(json_EmailYCC);

            if (validacion != null)
            {
                if (validacion.EmailExistente == true || validacion.CedulaExistente == true)
                {
                    resultadoVal = true;// el metodo encontro un usuario con la misma cedula o e-mail ingresados
                }
                else
                {
                    resultadoVal = false; // el metodo no encontro un usuario con la misma cedula o e-mail ingresados
                }

            }
            else
            {
                resultadoVal = false;
            }
            return resultadoVal;
        }

        /*
         Autor: Steven Cruz
         Desc: Servicio para registrar un usuario.
         Parms: Desde el body de un formulario del frontend viene un json con los atributos de usuario
         Return: boolean registrado
        */

        [HttpPost]
        [Route("registro")]
        public HttpResponseMessage Registro([FromBody] UUsuario usuario)
        {
            if (usuario == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var registrado = new LRegistro().Registrar(usuario);
            return Request.CreateResponse(HttpStatusCode.OK, registrado);
        }

    }
}
