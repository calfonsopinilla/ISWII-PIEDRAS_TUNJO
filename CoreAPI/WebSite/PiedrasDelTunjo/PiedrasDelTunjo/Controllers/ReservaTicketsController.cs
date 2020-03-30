using Data;
using Logica;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Utilitarios;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System.IO;
using System.Drawing;
using QRCoder;
using System;
using System.Drawing.Imaging;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("reserva-tickets")]
    public class ReservaTicketsController : ApiController
    {

        [HttpGet]
        [Route("")]
        // GET: reserva-tickets/
        public HttpResponseMessage ObtenerTickets()
        {
            var reservas = new LReservaTicket().ObtenerTickets();
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, results = reservas });
        }

        [HttpGet]
        [Route("")]
        // GET: reserva-tickets?user_id=5
        public HttpResponseMessage ObtenerPorUser([FromUri] int userId)
        {
            var reservas = new LReservaTicket().ObtenerPorUser(userId);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, results = reservas });
        }

        [HttpGet]
        [Route("{id}")]
        // GET: reserva-tickets/5
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var reserva = new LReservaTicket().Buscar(id);
            if (reserva == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Reserva no encontrada" });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, reserva });
        }

        [HttpPost]
        [Route("")]
        // POST: reserva-tickets/
        public HttpResponseMessage Crear([FromBody] UReservaTicket reserva)
        {
            if (reserva == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Reserva null" });
            }

            bool created = new LReservaTicket().NuevaReserva(reserva);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = created });
        }

        [HttpPut]
        [Route("{id}")]
        // PUT: reserva-tickets/5
        public HttpResponseMessage Editar([FromUri] int id, [FromBody] UReservaTicket reserva)
        {
            if (id != reserva.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Bad request" });
            }
            bool updated = new LReservaTicket().ActualizarReserva(id, reserva);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = updated });
        }

        [HttpDelete]
        [Route("{id}")]
        // DELETE: reserva-tickets/5
        public HttpResponseMessage Eliminar([FromUri] int id)
        {
            var reserva = new LReservaTicket().Buscar(id);
            if (reserva == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Reserva no encontrado" });
            }
            var removed = new LReservaTicket().EliminarReserva(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = removed });
        }

        [HttpGet]
        [Route("obtenerPrecio")]
        // GET: reserva-tickets/obtenerPrecio?userId
        public HttpResponseMessage ObtenerPrecio([FromUri] int userId)
        {
            double precio = new LReservaTicket().CalcularPrecio(userId);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, precio });
        }
    }
}
