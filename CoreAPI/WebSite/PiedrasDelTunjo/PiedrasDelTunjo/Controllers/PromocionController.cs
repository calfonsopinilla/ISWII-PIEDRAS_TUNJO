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
    [RoutePrefix("promocion")]
    public class PromocionController : ApiController
    {
        /*
        Daniel Zambrano
           Parámetros: Ninguno
           Retorna: Lista de preguntasfrecuentes.
       */

        [HttpGet]
        [Route("")]
        // GET: promocion/
        public IHttpActionResult ObtenerPromociones()
        {
            var promocion = new LPromocion().ObtenerPromociones();

            return Ok(promocion); // Status 200 OK
        }

        [HttpGet]
        [Route("{id}")]
        // GET: promocion/id
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var promocion = new LPromocion().Buscar(id);
            if (promocion == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "promocion no encontrada");
            }
            return Request.CreateResponse(HttpStatusCode.OK, promocion);
        }

        [HttpPost]
        [Route("")]
        // POST: promocion/
        public HttpResponseMessage Agregar([FromBody] UPromocion promocion)
        {
            if (promocion == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
            }

          
            promocion.Token = "";
            bool creado = new LPromocion().Agregar(promocion);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = creado });
        }

        [HttpPut]
        [Route("{id}")]
        // PUT: promocion/id
        public HttpResponseMessage Actualizar([FromUri] int id, [FromBody] UPromocion promocion)
        {
            if (id != promocion.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            promocion.Token = "";
            bool actualizado = new LPromocion().Actualizar(id, promocion);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }

        [HttpDelete]
        [Route("{id}")]
        // DELETE: promocion/id
        public HttpResponseMessage Eliminar([FromUri] int id)
        {
            var promocion = new LPromocion().Buscar(id);
            if (promocion == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "promocion no encontrada");
            }
            var eliminado = new LPromocion().Eliminar(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = eliminado });
        }
    }
}