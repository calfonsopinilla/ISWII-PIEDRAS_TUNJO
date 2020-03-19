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
    [RoutePrefix("preguntasfrecuentes")]
    public class Preguntas_frecuentesController : ApiController
    {
        /*
           Daniel Zambrano
           Parámetros: Ninguno
           Retorna: Lista de preguntasfrecuentes .
       */

        [HttpGet]
        [Route("")]
        // GET: preguntasfrecuentes/
        public IHttpActionResult ObtenerEventos()
        {
            var preguntas_frecuentes = new LPreguntas_frecuentes().ObtenerPreguntasFrecuentes();

            return Ok(preguntas_frecuentes); // Status 200 OK
        }

        [HttpGet]
        [Route("{id}")]
        // GET: preguntasfrecuentes/id
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var preguntas_frecuentes = new LPreguntas_frecuentes().Buscar(id);
            if (preguntas_frecuentes == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "pregunta frecuente no encontrado");
            }
            return Request.CreateResponse(HttpStatusCode.OK, preguntas_frecuentes);
        }

        [HttpPost]
        [Route("")]
        // POST: preguntasfrecuentes/
        public HttpResponseMessage Agregar([FromBody] UPreguntas_frecuentes preguntas_frecuentes)
        {
            if (preguntas_frecuentes == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
            }

       
            preguntas_frecuentes.Token = "";
            bool creado = new LPreguntas_frecuentes().Agregar(preguntas_frecuentes);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = creado });
        }

        [HttpPut]
        [Route("{id}")]
        // PUT: preguntasfrecuentes/id
        public HttpResponseMessage Actualizar([FromUri] int id, [FromBody] UPreguntas_frecuentes preguntas_frecuentes)
        {
            if (id != preguntas_frecuentes.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            preguntas_frecuentes.Token = "";
            bool actualizado = new LPreguntas_frecuentes().Actualizar(id, preguntas_frecuentes);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }

        [HttpDelete]
        [Route("{id}")]
        // DELETE: preguntasfrecuentes/5
        public HttpResponseMessage Eliminar([FromUri] int id)
        {
            var preguntas_frecuentes = new LPreguntas_frecuentes().Buscar(id);
            if (preguntas_frecuentes == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "pregunta no encontrado");
            }
            var eliminado = new LPreguntas_frecuentes().Eliminar(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = eliminado });
        }
    }
}
