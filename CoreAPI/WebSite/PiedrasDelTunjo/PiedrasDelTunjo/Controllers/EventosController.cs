using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web.Http.Cors;

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
        [Route("")]
        // GET: eventos/
        public IHttpActionResult ObtenerEventos()
        {
            var eventos = new LEvento().ObtenerEventos();
            eventos = eventos.Where(x => x.Fecha <= DateTime.Now.AddMonths(1))
                            .ToList();
            return Ok(eventos); // Status 200 OK
        }
    }
}
