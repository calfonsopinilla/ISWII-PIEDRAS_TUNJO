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
    public class PictogramasController : ApiController
    {
        /**
        * Autor: Mary Zapata
        * fecha: 19/03/2019
        * Parametro de recepcion: json tipo UPictograma, debe traer estado 1 para su registro
        * Return: string estado registro del Pictograma
        **/

        [HttpGet]
        [Route("Pictograma/Registro")]
        //public string RegistroPictograma([FromUri]string jsonRegistroPic)
        public IHttpActionResult RegistroPictograma([FromUri]string jsonRegistroPic)
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
        [Route("Pictograma/Ver_Pictogramas")]
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
         * Autor: Mary Zapata
         * fecha: 20/03/2019              
         * Return: string que indica un mensaje con respecto al estado del procedimiento de edicion
         **/
        [HttpGet]
        [Route("Pictograma/Editar_Pictograma")]
        //public string Editar_Pictograma([FromUri]string json_InfoNueva)
        public IHttpActionResult Editar_Pictograma([FromUri]string json_InfoNueva)
        {
            try
            {
                return Ok(new LPictograma().EditarPictograma(json_InfoNueva));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /**
         * Autor: Mary Zapata
         * fecha: 20/03/2019  
         * METODO para cambiar estado del pictograma y "eliminarlo"
         * Return: string que indica un mensaje con respecto al estado del procedimiento de cambio de estado
         **/
        [HttpGet]
        [Route("Pictograma/Remover_Pictograma")]
        //public string Remover_Pictograma([FromUri]string json_Info)
        public IHttpActionResult Remover_Pictograma([FromUri]string json_Info)
        {
            try
            {
                return Ok(new LPictograma().CambiarEstado_Pictograma(json_Info));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
