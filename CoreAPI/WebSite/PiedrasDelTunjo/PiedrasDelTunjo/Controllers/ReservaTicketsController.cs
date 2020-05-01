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

        /*
          * Autor: Jose Luis Soriano Roa 
          * Fecha de modificación: 21-04-2020
          * No resive parametros
          * Crea el qr de la entrada al parque y agrega la reserva
          * No es posible que un usuario obtenga dos tickets gratis para un mismo sea debido a excepcion por entrada de edad
          * Como tampoco es posible que el usuario tenga dos tickets para los residentes de faca en la misma fecha
      */

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
            try
            {
                QRCodeEncoder encoder = new QRCodeEncoder();
                Bitmap img = encoder.Encode(reserva.Qr);
                System.Drawing.Image QR = (System.Drawing.Image)img;
                using (MemoryStream ms = new MemoryStream())
                {
                    //opcional;
                    QR.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Byte[] imageBytes = ms.ToArray();
                    //imagen
                    Image imagen1 = (Bitmap)((new ImageConverter()).ConvertFrom(imageBytes));
                    imagen1.Save(HttpContext.Current.Server.MapPath($"~/Imagenes/Reserva/Tickets/{ reserva.Qr }.jpeg"));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = ex.Message });
            }
            reserva.FechaIngreso = reserva.FechaIngreso.Date;
            bool created = new LReservaTicket().NuevaReserva(reserva);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = created });
        }


        [HttpGet]
        [Route("getAgeUser")]
        // GET: reserva-tickets/getAgeUser?userId=149
        public HttpResponseMessage ObtenerEdad([FromUri] int userId)
        {
            var user = new LUsuario().Buscar(userId);
            int edad = new LReservaTicket().CalcularEdad(user.FechaNacimiento);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, edad });
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


        /*
       * Autor: Jose Luis Soriano Roa 
       * Fecha de modificación: 21-04-2020
       * id del usuario
       * evalua segun los datos del usuario si es residente de faca para permitir  o no la compra del ticket de residentes
       
   */



        [HttpGet]
        [Route("validarResidencia")]
        
        public HttpResponseMessage validarResidencia([FromUri] int userId)
        {
            bool residencia = new LReservaTicket().validarResidencia(userId);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, residencia });
        }



        /*
        * Autor: Jose Luis Soriano Roa 
        * Fecha de modificación: 21-04-2020
        * id del usuario
        * Evalua que el usuario pueda o no adquirir los tickets de entrada gratis

        */

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
        public HttpResponseMessage LeerToken([FromUri] string qr) {

            UReservaTicket reserva = new LReservaTicket().LeerToken(qr);

            if (reserva != null)
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, reserva });
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "ERROR: No se encontro la reserva" });
        }

        /*
            * Autor: Jhonattan Pulido
            * Descripcion: Método que funciona para buscar las reservas de un usuario filtrado por el id
            * Parámetros: String numeroDocumento: numero de documento del usuario
            * Retorna: La reserva buscada
            * Ruta: .../reserva-tickets/leerDNI?numeroDocumento=0000000
        */
        [HttpGet]
        [Route("leerDNI")]
        public HttpResponseMessage LeerDNI([FromUri] string numeroDocumento) {

            UReservaTicket reserva = new LReservaTicket().LeerReservaDNI(numeroDocumento);

            if (reserva != null)
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, reserva });
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "ERROR: No se encontraron reservas" });
        }

        /*
         * Autor: Jhonattan Pulido
         * Descripción: Método que funciona para gastar el qr de reserva
         * Parámetros: Int id: Identificador de la reserva
         * Retorna: True si la ejecución fue correcta - False si durante la ejecución ocurre un error
         * Ruta: .../reserva-tickets/validarQr?id=1
         */
        [HttpGet]
        [Route("validarQr")]        
        public HttpResponseMessage ValidarQr([FromUri] int id) {

            try {

                UReservaTicket reserva = new LReservaTicket().Buscar(id);

                if (reserva != null) {

                    reserva.EstadoId = 2;
                    bool actualizado = new LReservaTicket().ActualizarReserva(id, reserva);
                    if (actualizado)
                        return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Validación exitosa!" });
                    else
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Algo malo ha ocurrido!" });

                } else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: QR No valido!" });

            } catch (Exception ex) { return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Algo malo ha ocurrido!", exception = ex }); }

        }


        [HttpGet]
        [Route("validarFechas")]
        public HttpResponseMessage ValidarFechas([FromUri] int userId)
        {
            var fechas = new LReservaTicket().fechasValidas(userId);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, results = fechas });
        }


        [HttpGet]
        [Route("validarFechasUser")]
        public HttpResponseMessage ValidarFechasUser( [FromUri] int idUser, int idTicket   ){

            var fechas = new LReservaTicket().ObtenerDiasHabilesTicketUser(idUser, idTicket);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, results = fechas });

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
