using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilitarios;
using Logica;


namespace PiedrasDelTunjo.Controllers
{   
    /**
         * Autor: Gabriel Zapata
         * fecha: 18/03/2019
         * Desc: Controller para el CRUD de subscripciones
         * 
         */
    //
    public class SubscripcionController : ApiController
    {
        /**
         * Autor: Gabriel Zapata
         * fecha: 18/03/2019
         * Parametro de recepcion: json tipo USubscripcion
         * Return: string estado registro de la subscripcion
         **/

        [HttpGet]
        [Route("Subscripcion/Registro")]
        public string RegistroSubscripcion(string jsonRegistroSub)
        {
            try
            {                
                return new LSubscripcion().RegistroSubscripcion(jsonRegistroSub);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }





    }
}
