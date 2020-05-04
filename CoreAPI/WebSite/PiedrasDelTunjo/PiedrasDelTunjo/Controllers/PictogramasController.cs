using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilitarios;
using Logica;
using System.Web.Http.Cors;

namespace PiedrasDelTunjo.Controllers
{

    /**
         * Autor: Mary Zapata
         * fecha: 19/03/2019
         * Desc: Controller para el CRUD de Pictogramas
         * 
         */
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    [RoutePrefix("pictogramas")]
    public class PictogramasController : ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage ObtenerTodos()
        {
            var pictogramas = new LPictograma().ObtenerTodos();
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, pictogramas });
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var pictograma = new LPictograma().Buscar(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, pictograma });
        }

        [HttpPost]
        [Authorize]
        [Route("")]
        public HttpResponseMessage Crear([FromBody] UPictograma pic)
        {
            if (pic == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Bad request" });
            }
            var response = new LPictograma().Agregar(pic);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public HttpResponseMessage Actualizar([FromBody] UPictograma pic, [FromUri] int id)
        {
            if (id != pic.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Bad request" });
            }
            var response = new LPictograma().Actualizar(pic, id);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public HttpResponseMessage Eliminar([FromUri] int id)
        {
            var deleted = new LPictograma().Eliminar(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = deleted });
        }
    }
}
