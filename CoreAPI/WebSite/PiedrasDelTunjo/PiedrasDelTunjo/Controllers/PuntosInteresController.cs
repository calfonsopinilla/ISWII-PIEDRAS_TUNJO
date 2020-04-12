using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Utilitarios;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("puntos-interes")]
    public class PuntosInteresController : ApiController
    {

        [HttpGet]
        [Route("")]
        public HttpResponseMessage ObtenerTodos()
        {
            var puntos = new LPuntoInteres().ObtenerTodos();
            return Request.CreateResponse(HttpStatusCode.OK, puntos);
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 26/03/2020
            Descripción: Método que sirve para guardar un punto de interes en la tabla puntos interes
            Recibe: UPuntoInteres puntoInteres - Objeto con los atributos a guardar
            Retorna: Booleano y mensaje desriptivo
        */
        [HttpPost]
        [Authorize]
        [Route("")]
        public HttpResponseMessage CrearPuntoInteres([FromBody] UPuntoInteres puntoInteres) {
            puntoInteres.LastModification = DateTime.Now;
            puntoInteres.Token = "";
            if (new LPuntoInteres().CrearPuntoInteres(puntoInteres)) {
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Punto de interes CREADO satisfactoriamente"});
            } else
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, new { ok = false, message = "ERROR" });
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public HttpResponseMessage LeerPuntoInteres([FromUri] int id)
        {
            var puntointeres = new LPuntoInteres().LeerPuntoInteres(id);
            if (puntointeres == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "puntointeres no encontrado");
            }
            return Request.CreateResponse(HttpStatusCode.OK, puntointeres);
        }

        //
        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        // DELETE: 
        public HttpResponseMessage Eliminar([FromUri] int id)
        {
            var puntoi = new LPuntoInteres().LeerPuntoInteres(id);
            if (puntoi == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Punto no encontrado");
            }
            var eliminado = new LPuntoInteres().BorrarPuntoInteres(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = eliminado });
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public HttpResponseMessage Actualizar([FromUri] int id, [FromBody] UPuntoInteres puntoInteres)
        {
            if (id != puntoInteres.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            puntoInteres.Token = "";
            bool actualizado = new LPuntoInteres().Actualizar(id, puntoInteres);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }
    }
}
