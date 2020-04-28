using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web.Http.Cors;
using Utilitarios;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("eventos")]
    public class EventosController : ApiController
    {
        /*
            Steven Cruz
            Parámetros: Ninguno
            Retorna: Lista de eventos desde la fecha actual hasta dentro de un mes.
        */

        [HttpGet]
        //[Authorize]
        [Route("")]
        // GET: eventos/
        public IHttpActionResult ObtenerEventos()
        {
            var eventos = new LEvento().ObtenerEventos();
            eventos = eventos.Where(x => x.Fecha <= DateTime.Now.AddMonths(1))
                            .ToList();
            return Ok(eventos); // Status 200 OK
        }

        [HttpGet]
        [Route("{id}")]
        // GET: eventos/5
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var evento = new LEvento().Buscar(id);
            if (evento == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Evento no encontrado" });
            }
            return Request.CreateResponse(HttpStatusCode.OK, evento);
        }

        [HttpPost]
        //[Authorize]
        [Route("")]
        // POST: eventos/
        public HttpResponseMessage Agregar([FromBody] UEvento evento)
        {
            if (evento == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Evento null" });
            } else if (evento.FechaPublicacion < DateTime.Now) {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Fecha invalida" });
            }
            bool creado = new LEvento().Agregar(evento);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = creado });
        }

        [HttpPut]
        //[Authorize]
        [Route("{id}")]
        // PUT: eventos/5
        public HttpResponseMessage Actualizar([FromUri] int id, [FromBody] UEvento evento)
        {
            if (id != evento.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Bad Request" });
            }
            bool actualizado = new LEvento().Actualizar(id, evento);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }

        [HttpDelete]
        //[Authorize]
        [Route("{id}")]
        // DELETE: eventos/5
        public HttpResponseMessage Eliminar([FromUri] int id)
        {
            var evento = new LEvento().Buscar(id);
            if (evento == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Evento no encontrado" });
            }
            var eliminado = new LEvento().Eliminar(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = eliminado });
        }
    }
}
