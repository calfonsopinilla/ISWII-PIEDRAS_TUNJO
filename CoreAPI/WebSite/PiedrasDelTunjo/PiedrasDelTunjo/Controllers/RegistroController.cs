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
    [RoutePrefix("registro")]
    public class RegistroController : ApiController
    {
        /*
         * Autor: Steven Cruz
         * Fecha: 31/03/2020
         * Desc: Validar si el correo del registro ya existe
         */
        [HttpGet]
        [Route("existeCorreo")]
        public HttpResponseMessage ExisteCorreo([FromUri] string correo)
        {
            bool existe = new LCuenta().ExisteCorreo(correo);
            return Request.CreateResponse(HttpStatusCode.OK, new { existe });
        }

        /*
         * Autor: Steven Cruz
         * Fecha: 31/03/2020
         * Desc: Validar si el número de documento del registro ya existe
         */
        [HttpGet]
        [Route("existeNumeroDoc")]
        public HttpResponseMessage ExisteNumeroDoc([FromUri] string numeroDoc)
        {
            bool existe = new LCuenta().ExisteNumeroDoc(numeroDoc);
            return Request.CreateResponse(HttpStatusCode.OK, new { existe });
        }

        /*
         Autor: Steven Cruz
         Desc: Servicio para registrar un usuario.
         Parms: Desde el body de un formulario del frontend viene un json con los atributos de usuario
         Return: boolean registrado
        */

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Registro([FromBody] UUsuario usuario)
        {
            if (usuario == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var registrado = new LCuenta().Registrar(usuario);
            return Request.CreateResponse(HttpStatusCode.OK, registrado);
        }

    }
}
