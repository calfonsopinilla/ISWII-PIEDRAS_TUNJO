using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilitarios;
using Logica;
using Newtonsoft.Json;
using System.Web.Http.Cors;
using System.IO;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InformacionController : ApiController
    {

        /*
         @Autor : Jose Luis Soriano Roa
         *Fecha de creación: 11/03/2020
         *Descripcion : Servicio que envia la informacion de el inicio de la app.
         *Este metodo recibe : No resive parametros
         * Retorna: lista de la informacion del parque en formato json, dentro de ese json  si no hay registros retorna un null en string ("null").
         */

        [HttpGet]
        [Route("informacion")]
        public IHttpActionResult EnviarInformacion() {
            try
            {
                var informacion = JsonConvert.DeserializeObject<List<UInformacionParque>>(new LInformacion().informacionParque());
                return Ok(informacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("informacion/{id}")]
        public IHttpActionResult GetInfoById([FromUri] int id)
        {
            var item = new LInformacion().ObtenerInfoById(id);
            return Ok(item);
        }


        /*
         @Autor : Jose Luis Soriano Roa
         *Fecha de creación: 11/03/2020
         *Descripcion : Servicio que envia la informacion de el inicio de la app.
         *Este metodo recibe : No resive parametros
         * Retorna: lista de la informacion del parque en formato json, dentro de ese json 
         */


        [HttpGet]
        [Route("informacion/enviarInformacionInicioWeb")]

        public string enviarInformacionInicioWeb() {


            try {

                return new LInformacion().informacionParqueInicioWeb();
            }
            catch (Exception ex) {
                throw ex; 
            }
        }

    }
}
