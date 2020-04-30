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
            * Autor: Jose Luis Soriano  
            * Descripción: Insertar Promociones
            * Fecha de modificación: 21-04-2020
            * Parámetros: Objeto u promocion
            * Nota: No es posible tener dos promociones del mismo ticket  para ya sea un mismo rango de fechas o
            * que estas interfieran en otro rango.
        */
        [HttpPost]
        [Authorize]
        [Route("crear")]
        public HttpResponseMessage CrearPromocion([FromBody] UPromocion promocion) {

            

            if (promocion.FechaInicio.Date < DateTime.Now.Date){
                return Request.CreateErrorResponse(HttpStatusCode.Created, "Fecha inicio no valida");
            }
            else if (promocion.FechaFin.Date < promocion.FechaInicio.Date){
                return Request.CreateErrorResponse(HttpStatusCode.Created, "La fecha fin debe ser despues de la fecha inicio");
            }

            promocion.Estado = "1";
            promocion.LastModification = DateTime.Now;
            promocion.Token = "";
            bool resultado = new LPromocion().validarPromocion(promocion);

            if (resultado == true)
            {
                bool creado = new LPromocion().CrearPromocion(promocion);

                if (creado == true)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Created, "promocion  agregada");
                }
                else {
                    return Request.CreateErrorResponse(HttpStatusCode.Created, "ocurrio un error");
                }

            }
            else{
                return Request.CreateErrorResponse(HttpStatusCode.Created, "promocion  no agregada, verifique que no exista otra promocion en ese rango de fechas");
            }
        }

        /*
          * Autor: Jose Luis Soriano Roa 
          * Fecha de modificación: 21-04-2020
          * No resive parametros
          * Retorna la lista de las promociones  
      */
        [HttpGet]
        
        [Route("")]
        public HttpResponseMessage ObtenerPromociones()
        {

            var promociones = new LPromocion().ObtenerPromociones();
            return Request.CreateResponse(HttpStatusCode.OK, promociones);
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
        [Authorize]
        [Route("cambiar_estado")]
        public HttpResponseMessage CambiarEstado([FromUri] int id) {

            bool creado = new LPromocion().CambiarEstado(id);
            
            if (creado)
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Cambio de estado efectuado correctamente" });
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Ha ocurrido un error con el servidor" });
        }

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Actualza promocion y valida que no permita que dos promociones del mismo ticket se crucen de fecha
            * Fecha de modificación: 21/04/2020
            * Parámetros: Objeto upromocion
            *Retorna respuesta de que si ocurrio un error, incumple la validacionde las fechas o si en verdad fue insertado 
        */
        [HttpPut]
        [Authorize]
        [Route("editar")]
        public HttpResponseMessage Actualizar([FromBody] UPromocion promocion) {

            int id= promocion.Id;
            if (id != promocion.Id) {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (promocion.FechaInicio.Date < DateTime.Now.Date)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Created, "Fecha inicio no valida");
            }
            else if (promocion.FechaFin.Date < promocion.FechaInicio.Date)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Created, "La fecha fin debe ser despues de la fecha inicio");
            }


            promocion.LastModification = DateTime.Now;
            bool resultado = new LPromocion().validarPromocionUpdate(promocion);
            if (resultado == true){
                promocion.Token = "";
                bool actualizado = new LPromocion().Actualizar(id, promocion);
                if (actualizado == true) {
                    return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
                }
                else{
                    return Request.CreateResponse(HttpStatusCode.OK, new { ok = "No fue posible actualizar" });
                }
            }
            else {
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = "Ya existe una reserva en este rango de fechas" });

            }
            
        }


        [HttpDelete]
        [Authorize]
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
        */

    }
}
