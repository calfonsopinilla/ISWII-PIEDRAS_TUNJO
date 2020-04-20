using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("recorridos")]
    public class RecorridosController : ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage ObtenerRecorridos()
        {
            var recorridos = new LRecorrido().ObtenerTodos();
            return Request.CreateResponse(HttpStatusCode.OK, recorridos);
        }
    }
}
