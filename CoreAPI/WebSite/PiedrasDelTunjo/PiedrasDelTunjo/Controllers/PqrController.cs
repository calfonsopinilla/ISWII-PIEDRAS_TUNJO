using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using Utilitarios;
using Logica;
using System.Net;
using System;

namespace PiedrasDelTunjo.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("pqr")]
    //[Authorize]
    public class PqrController : ApiController
    {

        /*
            * Autor: Jhonattan Pulido
            * Descripcion: Método que funciona para responder el PQR de un usuario
            * Parametros: Int id: Indentificador unico del usuario, UPQR pqr: Objeto tipo pqr con los datos a actualizar
            * Retorna: Booleano true o false
            * Ruta: pqr/responder/{id}
        */
        

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
        public HttpResponseMessage Agregar([FromBody] UPQR pqr)
        {
            try {

                if (pqr == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "PQR null" });
                }
                pqr.FechaPublicacion = DateTime.Now;
                //pqr.UEstadoPQR.Id = 1;
                pqr.UEstadoPQRId = 1;
                pqr.FechaRespuesta = new DateTime();
                bool respuesta = new LPqr().agregarPqr(pqr);
                return Request.CreateResponse(HttpStatusCode.Created, new { ok = respuesta });

            } catch (Exception ex) { return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "Error Intero", exception = ex, pqr }); }
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

        [HttpPut]
        [Route("responder/{id}")]
        public HttpResponseMessage ResponderPQR([FromUri] int id, [FromBody] UPQR pqr)
        {

            if (id != pqr.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Bad Request" });
            }
            pqr.FechaRespuesta = DateTime.Now;
            //pqr.UEstadoPQR.Id = 2;
            pqr.UEstadoPQRId = 2;
           
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
