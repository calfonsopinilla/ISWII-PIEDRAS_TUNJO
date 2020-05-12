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
    [RoutePrefix("reserva-cabanas")]
    //[Authorize]
    public class ReservaCabanasController : ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage ObtenerTodos()
        {
            var reservas = new LReservaCabana().ObtenerTodos();
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, reservas });
        }

        ///Fecha : 13/04/2020
        /// Nombre: Jose Luis Soriano 
        /// Resive :Id de la cabaña
        /// Retorna : lista de las fechas disponibles para la reserva de la cabaña elegida

        [HttpGet]
        [Route("diasHabiles")]
        public HttpResponseMessage ObtenerDiasHabiles([FromUri] int id )
        {
            var reservas = new LReservaCabana().ObtenerDiasHabilesCabana(id) ;
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, reservas });
        }


        [HttpGet]
        [Route("")]
        // GET: reserva-cabanas?userId=5
        public HttpResponseMessage ObtenerPorUsuario([FromUri] int userId)
        {
            var reservas = new LReservaCabana().ObtenerPorUsuario(userId);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, reservas });
        }


        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var reserva = new LReservaCabana().Buscar(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, reserva });
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage NuevaReserva([FromBody] UReservaCabana reserva)
        {
            var created = new LReservaCabana().Agregar(reserva);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = created });
        }

        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage EliminarReserva([FromUri] int id)
        {
            var deleted = new LReservaCabana().Eliminar(id);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = deleted });
        }
    }
}
