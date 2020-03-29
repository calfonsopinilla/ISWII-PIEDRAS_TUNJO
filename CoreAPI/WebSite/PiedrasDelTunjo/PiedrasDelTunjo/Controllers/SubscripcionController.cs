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
         * Autor: Gabriel Zapata
         * fecha: 18/03/2019
         * Desc: Controller para el CRUD de subscripciones
         * 
         */
    //
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    public class SubscripcionController : ApiController
    {
        /**
         * Autor: Gabriel Zapata
         * fecha: 18/03/2019
         * Parametro de recepcion: json tipo USubscripcion, debe traer estado 1 para su registro
         * Return: string estado registro de la subscripcion
         **/

        [HttpGet]
        [Route("Subscripcion/Registro")]
        //public string RegistroSubscripcion([FromUri]string jsonRegistroSub)
        public IHttpActionResult RegistroSubscripcion([FromUri]string jsonRegistroSub)
        {
            try
            {                
                return Ok(new LSubscripcion().RegistroSubscripcion(jsonRegistroSub));
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        /**
         * Autor: Gabriel Zapata
         * fecha: 19/03/2019              
         * Return: string json que contiene una lista con todos los registros de subscripciones
         *  traidos de la base de datos
         **/
        [HttpGet]
        [Route("Subscripcion/Ver_Subscripciones")]
        //public List<USubscripcion> MostrarSubscripciones([FromUri]int estadoFiltro)
        public IHttpActionResult MostrarSubscripciones([FromUri]int estadoFiltro)
        {
            try
            {
                var informacion = new LSubscripcion().Mostrar_Subscripciones(estadoFiltro);
                return Ok(informacion);

            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /**
         * Autor: Gabriel Zapata
         * fecha: 19/03/2019              
         * Return: string que indica un mensaje con respecto al estado del procedimiento de edicion
         **/
        [HttpGet]
        [Route("Subscripcion/Editar_Subscripciones")]
        //public string Editar_Subscripciones([FromUri]string json_InfoNueva)
        public IHttpActionResult Editar_Subscripciones([FromUri]string json_InfoNueva)
        {
            try
            {
                return Ok(new LSubscripcion().EditarSubscripcion(json_InfoNueva));
                    
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /**
         * Autor: Gabriel Zapata
         * fecha: 19/03/2019  
         * METODO para cambiar estado de la subscripcion y "eliminarla"
         * Return: string que indica un mensaje con respecto al estado del procedimiento de cambio de estado
         **/
        [HttpGet]
        [Route("Subscripcion/Remover_Subscripciones")]
        //public string Remover_Subscripciones([FromUri]string json_Info)
        public IHttpActionResult Remover_Subscripciones([FromUri]string json_Info)
        {
            try
            {
                return Ok(new LSubscripcion().CambiarEstado_Subscripcion(json_Info));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
