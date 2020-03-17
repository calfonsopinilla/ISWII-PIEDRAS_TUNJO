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


        /*
            Steven Cruz
            Parámetros: Nombre de la imagen.
            Retorna: La imagen con el nombre especificado en la ruta.
        */
        [HttpGet]
        [Route("informacion/obtenerImagen")]
        // GET: eventos/obtenerImagen/?nombre=imageName.jpg
        public HttpResponseMessage ObtenerImagen([FromUri] string nombre)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var path = $"~/Imagenes/Informacion/{ nombre }";
            path = System.Web.Hosting.HostingEnvironment.MapPath(path);
            var ext = Path.GetExtension(path);
            ext = ext.Substring(1); // para que elimine el punto (.) de la extensión
            var contents = File.ReadAllBytes(path);

            MemoryStream ms = new MemoryStream(contents);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue($"image/{ext}");
            return response;
        }

    }
}
