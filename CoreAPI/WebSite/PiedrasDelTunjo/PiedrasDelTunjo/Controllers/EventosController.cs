using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;

namespace PiedrasDelTunjo.Controllers
{
    [RoutePrefix("eventos")]
    public class EventosController : ApiController
    {
        /*
            Steven Cruz
            Parámetros: Ninguno
            Retorna: Lista de eventos desde la fecha actual hasta dentro de un mes.
        */

        [HttpGet]
        [Route("")]
        // GET: eventos/
        public IHttpActionResult ObtenerEventos()
        {
            var eventos = new LEvento().ObtenerEventos();
            eventos = eventos.Where(x => x.Fecha <= DateTime.Now.AddMonths(1))
                            .ToList();
            return Ok(eventos); // Status 200 OK
        }


        /*
            Steven Cruz
            Parámetros: Nombre de la imagen del evento.
            Retorna: La imagen con el nombre especificado en la ruta.
        */
        [HttpGet]
        [Route("obtenerImagen")]
        // GET: eventos/obtenerImagen/?nombre=imageName.jpg
        public HttpResponseMessage ObtenerImagen([FromUri] string nombre)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var path = $"~/Imagenes/Eventos/{nombre}";
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
