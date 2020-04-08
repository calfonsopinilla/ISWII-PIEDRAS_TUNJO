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

    /**
         * Autor: Mary Zapata
         * fecha: 19/03/2019
         * Desc: Controller para el CRUD de Pictogramas
         * 
         */
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    [RoutePrefix("Pictograma")]
    public class PictogramasController : ApiController
    {
        /**
        * Autor: Mary Zapata
        * fecha: 19/03/2019
        * Parametro de recepcion: json tipo UPictograma, debe traer estado 1 para su registro
        * Return: string estado registro del Pictograma
        **/

        [HttpPost]
        [Route("Registro")]
        //public string RegistroPictograma([FromUri]string jsonRegistroPic)
        public IHttpActionResult RegistroPictograma([FromBody] UPictograma jsonRegistroPic)
        {
            try
            {
                return Ok(new LPictograma().RegistroPictograma(jsonRegistroPic));
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        /**
        * Autor: Mary Zapata
        * fecha: 19/03/2019              
        * Return: string json que contiene una lista con todos los registros de pictogramas
        *  traidos de la base de datos
        **/
        [HttpGet]
        [Route("Ver_Pictogramas")]
        public IHttpActionResult MostrarPictogramas([FromUri]int estadoFiltro)
        {
            try
            {
                

                var informacion = new LPictograma().Mostrar_Pictogramas(estadoFiltro);

                return Ok(informacion);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /**
         * 
         * 
         * **/
        [HttpGet]
        [Route("{id}")]
        // GET: Usuarios/id
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var pictograma = new LPictograma().LeerPictograma(id);
            if (pictograma == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Pictograma no encontrado");
            }
            return Request.CreateResponse(HttpStatusCode.OK, pictograma);
        }



        /**
         * Autor: Mary Zapata
         * fecha: 20/03/2019              
         * Return: string que indica un mensaje con respecto al estado del procedimiento de edicion
         **/
        [HttpPut]
        [Route("{id}")]
        //public string Editar_Pictograma([FromUri]string json_InfoNueva)
     
        public HttpResponseMessage Editar_Pictograma([FromUri] int id, [FromBody] UPictograma json_InfoNueva)
        {
            if (id != json_InfoNueva.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
           
            bool actualizado = new LPictograma().Actualizar(id, json_InfoNueva);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }
        /**
         * Autor: Mary Zapata
         * fecha: 20/03/2019  
         * METODO para cambiar estado del pictograma y "eliminarlo"
         * Return: string que indica un mensaje con respecto al estado del procedimiento de cambio de estado
         **/
        [HttpGet]
        [Route("Remover_Pictograma")]
        //public string Remover_Pictograma([FromUri]string json_Info)
        public IHttpActionResult Remover_Pictograma([FromUri]int id_pictograma)
        {
            try
            {
                return Ok(new LPictograma().CambiarEstado_Pictograma(id_pictograma));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
