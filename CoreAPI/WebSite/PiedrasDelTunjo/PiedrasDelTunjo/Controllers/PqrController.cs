using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using Utilitarios;
using Logica;
namespace PiedrasDelTunjo.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("pqr")]
    public class PqrController : ApiController
    {
        /*
            Jose Luis Soriano
            Parámetros: Ninguno
            Retorna: Lista de pqr
        */

        [HttpGet]
        [Route("")]
        // GET: pqr/
        public HttpResponseMessage ObtenerPqr() {

            var pqrs = new LPqr().ObtenerPqr();
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, results = pqrs });
        }

        [HttpGet]
        [Route("")]
        // GET: pqr?userId=5
        public HttpResponseMessage ObtenerPorUser([FromUri] int userId)
        {
            var pqrs = new LPqr().ObtenerPorUser(userId);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, results = pqrs });
        }

        [HttpGet]
        [Route("{id}")]
        // GET: pqr/5
        public HttpResponseMessage BuscarPqr([FromUri] int id)
        {
            var pqr = new LPqr().BuscarPqr(id);

            if (pqr == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "pqr no encontrado" });
            }
            else {
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, pqr });
            }
        }

        /*
        Jose Luis Soriano
        Parámetros: objeto UPQr
        Retorna: true si fue agregado false si no  
        */

        [HttpPost]
        [Route("")]
        // POST: pqr/
        public HttpResponseMessage Agregar([FromBody] UPQR upqr)
        {
            if (upqr == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "PQR null" });
            }
            upqr.FechaPublicacion = DateTime.Now;
            bool respuesta = new LPqr().agregarPqr(upqr);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = respuesta });
        }
        /*
        Jose Luis Soriano
        Parámetros: ojeto UPQr y id 
        Retorna: true si fue actualizado false si no  
        */

        [HttpPut]
        [Route("{id}")]
        // PUT: pqr/5
        public HttpResponseMessage ActualizarPqr([FromUri] int id, [FromBody] UPQR pqr)
        {
            if (id != pqr.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Bad Request" });
            }
            bool actualizado = new LPqr().actualizaPqr(id, pqr);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }

        [HttpDelete]
        [Route("{id}")]

        /*
       Jose Luis Soriano
       Parámetros:  id 
       Retorna: true si fue eliminado false si no  
       */

        // DELETE: pqr/5
        public HttpResponseMessage EliminarPqr([FromUri] int id)
        {
            var pqr = new LPqr().BuscarPqr(id);

            if (pqr == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Pqr no encontrado" });
            }
            else {
                var eliminado = new LPqr().eliminarPqr(id);
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = eliminado });
            }
            
        }

    }
}
