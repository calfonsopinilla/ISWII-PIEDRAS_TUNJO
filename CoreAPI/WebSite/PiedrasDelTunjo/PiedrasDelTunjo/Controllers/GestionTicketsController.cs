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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("GestionTickets")]
    //[Authorize]
    public class GestionTicketsController : ApiController
    {
        /**
        * Autor: Gabriel Zapata
        * fecha: 14/04/2020              
        * Return: lista con todos los tipos de tickets dependiendo del filtro
        *  traidos de la base de datos
        **/
        [HttpGet]
        [Route("Ver_Tickets")]        
        public IHttpActionResult MostrarTickets([FromUri]int estadoFiltro)
        {
            try
            {
                var informacion = new LGestionTickets().ObtenerTickets_Filtrados(estadoFiltro);
                return Ok(informacion);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /**
        * Autor: Gabriel Zapata
        * fecha: 14/04/2020   
        * Parametro de recepcion: objeto tipo USubscripcion, debe traer estado 1 para su registro
        * Return: string estado registro del ticket
        **/

        [HttpPost]
        [Route("Agregar")]
        [Authorize]
        public IHttpActionResult RegistroTickets([FromBody]UTicket datosTicket)
        {
            try
            {
                datosTicket.Token = Guid.NewGuid().ToString();
                return Ok(new LGestionTickets().RegistroTicket(datosTicket));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /**
         * Autor: Gabriel Zapata
         * fecha: 14/04/2020  
         *Descripcion: busca el ticket a editar
         *Recibe: una estreuctura json para agregar los datos
         *Retorna: un true para saber que se agregaron los datos
        */
        /*
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public IHttpActionResult BuscarTicket([FromUri] int id)
        {
            return Ok(new LGestionTickets().Buscar(id));
        }

            */
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public HttpResponseMessage BuscarTicket([FromUri] int id)
        {
            //return Ok(new LGestionTickets().Buscar(id));
            var informacion = new LGestionTickets().Buscar(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, informacion });
        }


        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage ActualizarTicket([FromBody] UTicket ticket)
        {            
            ticket.Token = Guid.NewGuid().ToString();
            bool actualizado = new LGestionTickets().Actualizar(ticket.Id, ticket);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }

        /**
         * Autor: Gabriel Zapata
         * fecha: 14/04/2020  
          *Descripcion: metodo para cambiar el estado del tipo de ticket
          *Recibe: id del ticket
          *Retorna: bool de confirmacion
          **/
        [HttpGet]
        [Route("Inhabilitar")]
        [Authorize]
        public bool EliminarTicket([FromUri]int id)
        {
            try
            {
                string token = Guid.NewGuid().ToString();
                return new LGestionTickets().EliminarTicket(id, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /**
        * Autor: Gabriel Zapata
        * fecha: 14/04/2020  
         *Descripcion: metodo para cambiar el estado del tipo de ticket
         *Recibe: id del ticket
         *Retorna: bool de confirmacion
         **/
        [HttpGet]
        [Route("Habilitar")]
        [Authorize]
        public bool HabilitarTicket([FromUri]int id, [FromUri]int estado)
        {
            try
            {
                string token = Guid.NewGuid().ToString();
                return new LGestionTickets().HabilitarTicket(id, estado, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
