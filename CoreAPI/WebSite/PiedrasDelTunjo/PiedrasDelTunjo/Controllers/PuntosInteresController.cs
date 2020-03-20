using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PiedrasDelTunjo.Controllers
{
    [RoutePrefix("puntos-interes")]
    public class PuntosInteresController : ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage ObtenerTodos()
        {
            var puntos = new LPuntoInteres().ObtenerTodos();
            return Request.CreateResponse(HttpStatusCode.OK, puntos);
        }
    }
}
