using Logica;
using System.Web.UI;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Utilitarios;
using System.IO;
using System.Drawing;
using System;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using MessagingToolkit.QRCode.Codec;
using System.Web;

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
        [Route("crear")]
        public HttpResponseMessage GenerarQr([FromBody] UReservaTicket reserva)
        {


            if (reserva == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Reserva null" });
            }

            reserva.Token = this.Encriptar(JsonConvert.SerializeObject(reserva));
            reserva.Qr = this.Encriptar(JsonConvert.SerializeObject(reserva));
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap img = encoder.Encode(reserva.Token);
            System.Drawing.Image QR = (System.Drawing.Image)img;
            using (MemoryStream ms = new MemoryStream())
            {
                //opcional;
                QR.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                Byte[] imageBytes = ms.ToArray();
                //imagen
                Image imagen1 = (Bitmap)((new ImageConverter()).ConvertFrom(imageBytes));
                imagen1.Save(HttpContext.Current.Server.MapPath($"~/Imagenes/Reserva/Tickets/{ reserva.Token }.jpeg"));
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


        [HttpGet]
        [Route("validarResidencia")]
        // GET: reserva-tickets/obtenerPrecio?userId
        public HttpResponseMessage validarResidencia([FromUri] int userId)
        {
            bool residencia = new LReservaTicket().validarResidencia(userId);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, residencia });
        }

        [HttpGet]
        [Route("validarEdad")]
        // GET: reserva-tickets/obtenerPrecio?userId
        public HttpResponseMessage validarEdad([FromUri] int userId)
        {
            bool edad = new LReservaTicket().validarEdades(userId);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, edad });
        }


        /*
         * Autor: Jhonattan Pulido
         * Descripcion: Método que funciona para buscar una reserva filtrado por ticket
         * Parametros: String token - Valor del token para filtrar
         * Retorna: Objeto tipo reserva token
         */
        [HttpGet]
        [Route("leerToken")]
        public HttpResponseMessage LeerToken([FromUri] string token) {

            UReservaTicket reserva = new LReservaTicket().LeerToken(token);

            if (reserva != null)
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, reserva });
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "ERROR: No se encontro la reserva" });
        }

        [HttpGet]
        [Route("validarFechas")]


        public HttpResponseMessage ValidarFechas() {
            var fechas = new LReservaTicket().fechasValidas();
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, results = fechas});

        }







        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 11/03/2020
            Descripción: Método que sirve para encriptar una cadena de texto
            Recibe: String input - Puede ser una clave, un token, etc.
            Retorna: La cadena encriptada
        */
        public string Encriptar(string input) {

            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }
    }
}
