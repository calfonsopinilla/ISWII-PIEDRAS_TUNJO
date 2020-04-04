using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Logica;
using Utilitarios;
namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("tickets")]
    public class TicketController : ApiController
    {

        /*
           Jose Luis Soriano
           Parámetros: Ninguno
           Retorna: Lista de ticktes 
       */

        [HttpGet]
        [Route("")]
        // GET: tickets/
        public HttpResponseMessage ObtenerTickets()
        {
            var ticket = new LTicktet().ObtenerTicktes();
            return Request.CreateResponse(HttpStatusCode.OK, ticket);
        }


        [HttpGet]
        [Route("{id}")]
        // GET: tickets/5
        public HttpResponseMessage BuscarTicket([FromUri] int id)
        {
            var ticket = new LTicktet().BuscarTicket(id);

            if (ticket== null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "ticket no encontrado");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, ticket);
            }
        }


        /*
        Jose Luis Soriano
        Parámetros: ojeto UTicket
        Retorna: true si fue agregado false si no  
        */


        [HttpPost]
        [Route("")]
        // POST: tickets/
        public HttpResponseMessage Agregarticket([FromBody] UTicket ticket)
        {
            if (ticket== null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
            }

            ticket.LastModificacion = DateTime.Now;
            ticket.Token = "";
            bool respuesta = new LTicktet().agregarTicktet(ticket);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = respuesta });
        }
        /*
        Jose Luis Soriano
        Parámetros: ojeto Uticket y id 
        Retorna: true si fue actualizado false si no  
        */

        [HttpPut]
        [Route("{id}")]
        // PUT: tickets/3
        public HttpResponseMessage ActualizarTicket([FromUri] int id, [FromBody] UTicket ticket)
        {
            if (id != ticket.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            ticket.Token = "";
            ticket.LastModificacion = DateTime.Now;
            bool actualizado = new LTicktet().actualizaticktet(id, ticket);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }

        [HttpDelete]
        [Route("{id}")]

        /*
       Jose Luis Soriano
       Parámetros:  id 
       Retorna: true si fue eliminado false si no  
       */

        // DELETE: tickets/5
        public HttpResponseMessage EliminarTicket([FromUri] int id)
        {
            var ticket = new LTicktet().BuscarTicket(id);

            if (ticket== null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Evento no encontrado");
            }
            else
            {
                var eliminado = new LTicktet().eliminarTicket(id);
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = eliminado });
            }

        }

    }
}
