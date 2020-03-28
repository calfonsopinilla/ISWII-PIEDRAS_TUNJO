using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Utilitarios;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("puntos-interes")]
    public class PuntosInteresController : ApiController
    {
        // Variables
        private UPuntoInteres puntoInteres;

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
        //[Route("crear")]
        [Route("")]
        public HttpResponseMessage CrearPuntoInteres([FromBody] UPuntoInteres puntoInteres) {
            puntoInteres.LastModification = DateTime.Now;
            puntoInteres.Token = "";
            if (new LPuntoInteres().CrearPuntoInteres(puntoInteres)) {
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Punto de interes CREADO satisfactoriamente"});
            } else
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, new { ok = false, message = "ERROR" });
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 26/03/2020
            Descripción: Método que sirve para guardar un punto de interes en la tabla puntos interes
            Recibe: Integer id - Id del punto de interes que se desea buscar
            Retorna: Objeto de tipo UPuntoInteres con los datos filtrados y booleano
        */
        /*public HttpResponseMessage LeerPuntoInteres([FromUri] int id) {

            this.puntoInteres = new LPuntoInteres().LeerPuntoInteres(id);

            if (this.puntoInteres != null) // Se valida si el punto de interes SI se encontro
                return Request.CreateResponse(HttpStatusCode.OK/*, new { ok = true, this.puntoInteres ,message=""});
            else
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, new { ok = false , message = "ERROR" });
        }*/
        [HttpGet]
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

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 26/03/2020
            Descripción: Método que sirve para guardar un punto de interes en la tabla puntos interes
            Recibe: Integer id - Id del punto de interes que se desea buscar
            Retorna: Booleano y mensaje desriptivo
        */
        [HttpGet]
        [Route("borrar")]
        public HttpResponseMessage BorrarPuntoInteres([FromUri] int id) {

            if (new LPuntoInteres().BorrarPuntoInteres(id))                
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Punto de interes ELIMINADO satisfactoriamente" });
            else
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, new { ok = false, message = "ERROR" });
        }

        //
        [HttpDelete]
        [Route("{id}")]
        // DELETE: 
        public HttpResponseMessage Eliminar([FromUri] int id)
        {
            var puntoi = new LPuntoInteres().LeerPuntoInteres(id);
            if (puntoi == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Evento no encontrado");
            }
            var eliminado = new LPuntoInteres().BorrarPuntoInteres(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = eliminado });
        }
        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 26/03/2020
            Descripción: Método que sirve para guardar un punto de interes en la tabla puntos interes
            Recibe: UPuntoInteres puntoInteres - Objeto con los atributos a actualizar
            Retorna: Booleano y mensaje desriptivo
        */
        [HttpPost]
        [Route("actualizar")]
        public HttpResponseMessage ActualizarPuntoInteres([FromBody] UPuntoInteres puntoInteres) {

            if (new LPuntoInteres().ActualizarPuntoInteres(puntoInteres)) {
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Punto de interes ACTUALIZADO satisfactoriamente"});
            } else
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, new { ok = false, message = "ERROR" });
        }
        ///
        [HttpPut]
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
