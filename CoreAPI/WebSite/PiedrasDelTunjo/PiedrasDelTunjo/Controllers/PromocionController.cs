using Logica;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web.Http.Cors;
using Utilitarios;

namespace PiedrasDelTunjo.Controllers {

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("promocion")]
    public class PromocionController : ApiController {

        private List<UPromocion> listaPromociones;

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para crear promociones
            * Fecha de modificación: 15-04-2020
            * Parámetros: UPromocion nuevaPromocion - Objeto con los datos de la promoción
            * Retorna: True si la promoción se almaceno correctamente - False si ocurrio un error en la ejecución del método
            * Ruta promocion/crear
        */
        [HttpPost]
        //[Authorize]
        [Route("crear")]
        public HttpResponseMessage CrearPromocion([FromBody] UPromocion promocion) {

            promocion.Estado = "1";
            promocion.LastModification = DateTime.Now;
            promocion.Token = "";
            bool creado = new LPromocion().CrearPromocion(promocion);

            if (creado)
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Promoción agregada correctamente" });
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Ha ocurrido un error con el servidor" });
        }

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para leer todas las promociones
            * Fecha de modificación: 15-04-2020
            * Parámetros: string estado - "1" si se quieren obtener las promociones habilitadas, "2" si se quieren obtener las promociones deshabilitadas
            * Retorna: Lista tipo UPromocion con las promociones - Null si ocurrio un error en la ejecución del método
            * Ruta: promocion/leer?estado=1 || promocion/leer?estado=2
        */
        [HttpGet]
        //[Authorize]
        [Route("leer")]
        public HttpResponseMessage LeerPromociones([FromUri] string estado) {

            this.listaPromociones = new LPromocion().LeerPromociones(estado);
            return Request.CreateResponse(HttpStatusCode.OK, new { promociones = this.listaPromociones });
        }

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para cambiar el estado de una promoción
            * Fecha de modificación: 15-04-2020
            * Parámetros: Int id - Identificador de la promoción
            * Retorna: True si el cambio de estado se efectuó correctamente - False si ocurrio un error en la ejecución del método
            * Ruta: promocion/cambiar_estado?id=1
        */
        [HttpGet]
        //[Authorize]
        [Route("cambiar_estado")]
        public HttpResponseMessage CambiarEstado([FromUri] int id) {

            bool creado = new LPromocion().CambiarEstado(id);

            if (creado)
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Cambio de estado efectuado correctamente" });
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Ha ocurrido un error con el servidor" });
        }

        [HttpPut]
        //[Authorize]
        [Route("{id}")]
        // PUT: promocion/id
        public HttpResponseMessage Actualizar([FromUri] int id, [FromBody] UPromocion promocion) {

            if (id != promocion.Id) {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            promocion.Token = "";
            bool actualizado = new LPromocion().Actualizar(id, promocion);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }

        /*
            Daniel Zambrano
           Parámetros: Ninguno
           Retorna: Lista de promociones.
       */
        /*[HttpGet]
        [Route("")]
        // GET: promocion/
        public IHttpActionResult ObtenerPromociones() {
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
        }*/
    }
}
