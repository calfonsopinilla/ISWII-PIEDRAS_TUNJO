using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilitarios;
using Logica;
using Newtonsoft.Json;

namespace PiedrasDelTunjo.Controllers
{
    public class RegistroController : ApiController
    {
        [HttpGet]
        [Route("Registro/Val_EmailYCC")]
        /**
         * Metodo para la validacion de la existencia del correo electronico y la cedula
         * en la bd
         */
        //
        public Boolean Val_EmailYCC(string valCorreo, string valDocumento)
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


    }
}
