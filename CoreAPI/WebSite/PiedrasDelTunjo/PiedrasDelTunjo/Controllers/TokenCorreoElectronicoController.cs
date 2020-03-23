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

        [HttpGet]
        [Route("usuario/registro/m")]
        public String EnviarToken()
        {            
            return "Hola";
        }

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
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, item });
        }
    }
}
