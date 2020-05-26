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
using System.Web;
using System.Diagnostics;

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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        [Route("informacion")]
        public IHttpActionResult EnviarInformacion() {
            try
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(HttpContext.Current.Server.MapPath("~/log4net.config")));
                Process currentProcess = Process.GetCurrentProcess();
                log.Info($"Inicia proceso #  {currentProcess.Id}");
                var informacion = JsonConvert.DeserializeObject<List<UInformacionParque>>(new LInformacion().informacionParque());
                log.Info($"Finaliza proceso #  {currentProcess.Id}");
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

        /**
         * Autor: Gabriel Zapata
         * fecha: 28/04/2020
         * Parametro de recepcion: la info a modificar de terminos y condiciones
         * Return: bool 
         **/
        [HttpPut]
        [Route("informacion/Actualizar")]
        [Authorize]
        public HttpResponseMessage ActualizarTerminosYCond([FromBody] UInformacionParque infoParque)
        {
            infoParque.Token = Guid.NewGuid().ToString();
            infoParque.LastModification = DateTime.Now;
            bool actualizado = new LInformacion().ActualizarTerminosYCond(infoParque);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }


    }
}
