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
    [RoutePrefix("Reportes")]
    public class InfoReportController : ApiController
    {

        [HttpGet]
        [Route("ReporteTickets")]        
        public HttpResponseMessage ObtenerInfoTickets([FromUri] DateTime fecha, [FromUri] int tipoTicket, [FromUri] int tipoFiltro)
        {
            var ticket = new LReservaTicket().ObtenerVendidos_TicketsFechaYTipo(fecha, tipoTicket, tipoFiltro);
            return Request.CreateResponse(HttpStatusCode.OK, ticket);
        }



    }
}
