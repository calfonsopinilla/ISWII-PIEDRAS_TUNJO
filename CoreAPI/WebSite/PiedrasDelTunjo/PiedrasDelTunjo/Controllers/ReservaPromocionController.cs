// Librerías
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Utilitarios;
using Logica;

namespace PiedrasDelTunjo.Controllers {

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("reserva_promocion")]
    public class ReservaPromocionController : ApiController {        

        /*
           * Autor: Jhonattan Pulido
           * Descripción: Método que sirve para agregar reserva de promociones
           * Fecha de modificación: 16-04-2020
           * Parámetros: UReservaPromocion nuevaPromocion: Objeto con los datos de la promocion que se quiere agregar
           * Retorna: True si la reserva se guardo correctamente - False si ocurrio un error en la ejecución del método
           * Ruta: reserva_promocion/crear
        */
        [HttpPost]
        //[Authorize]
        [Route("crear")]
        public HttpResponseMessage CrearReserva([FromBody] UReservaPromocion promocion) {

            promocion.LastModification = DateTime.Now;
            promocion.Token = "";
            promocion.EstadoId = 1;

            bool creado = new LReservaPromocion().CrearReserva(promocion);

            if (creado)
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Promoción agregada correctamente" });
            else
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Ha ocurrido un error con el servidor" });
        }

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para leer las promociones compradas por un usuario
            * Fecha de modificación: 16-04-2020
            * Parámetros: Int id : Identificador del usuario del cual se quieren ver las promociones compradas
            * Retorna: Lista de las promociones compradas por el usurio - Null si ocurrio un error en la ejecución del método
            * Ruta: reserva_promocion/leer_usuario?id=1
        */
        [HttpGet]
        //[Authorize]
        [Route("leer_usuario")]
        public HttpResponseMessage LeerPromocionesUsuario([FromUri] int id) {

            var listaPromociones = new LReservaPromocion().LeerPromocionesUsuario(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { promociones = listaPromociones });
        }
    }
}
