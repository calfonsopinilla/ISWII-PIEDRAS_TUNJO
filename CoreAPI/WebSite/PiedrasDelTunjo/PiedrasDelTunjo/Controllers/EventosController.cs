﻿using Logica;
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
        [Route("")]
        // GET: eventos/
        public HttpResponseMessage ObtenerEventos()
        {
            var eventos = new LEvento().ObtenerEventos();
            eventos = eventos.Where(x => x.Fecha <= DateTime.Now.AddMonths(1))
                            .ToList();
            return Request.CreateResponse(HttpStatusCode.OK, eventos); // Status 200 OK
        }

        [HttpGet]
        [Route("{id}")]
        // GET: eventos/5
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var evento = new LEvento().Buscar(id);
            if (evento == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Evento no encontrado");
            }
            return Request.CreateResponse(HttpStatusCode.OK, evento);
        }

        [HttpPost]
        [Route("")]
        // POST: eventos/
        public HttpResponseMessage Agregar([FromBody] UEvento evento)
        {
            if (evento == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
            }

            evento.FechaPublicacion = DateTime.Now;
            evento.Token = "";
            bool creado = new LEvento().Agregar(evento);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = creado });
        }

        [HttpPut]
        [Route("{id}")]
        // PUT: eventos/5
        public HttpResponseMessage Actualizar([FromUri] int id, [FromBody] UEvento evento)
        {
            if (id != evento.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            evento.Token = "";
            bool actualizado = new LEvento().Actualizar(id, evento);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }

        [HttpDelete]
        [Route("{id}")]
        // DELETE: eventos/5
        public HttpResponseMessage Eliminar([FromUri] int id)
        {
            var evento = new LEvento().Buscar(id);
            if (evento == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Evento no encontrado");
            }
            var eliminado = new LEvento().Eliminar(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = eliminado });
        }
    }
}
