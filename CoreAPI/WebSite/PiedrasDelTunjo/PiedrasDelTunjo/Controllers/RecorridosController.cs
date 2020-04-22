using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Utilitarios;

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
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, recorridos });
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Crear([FromBody] URecorrido recorrido)
        {
            if (recorrido == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Bad request" });
            }
            recorrido.Token = Guid.NewGuid().ToString();
            recorrido.LastModification = DateTime.Now;
            bool created = new LRecorrido().Agregar(recorrido);
            return Request.CreateResponse(HttpStatusCode.OK, new {ok  = true, message = "Created successfully!" });
        }

        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Eliminar([FromUri] int id)
        {
            var recorrido = new LRecorrido().Buscar(id);
            if (recorrido == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "bad request" });
            }
            bool deleted = new LRecorrido().Eliminar(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = deleted, message = "Deleted!" });
        }
    }
}
