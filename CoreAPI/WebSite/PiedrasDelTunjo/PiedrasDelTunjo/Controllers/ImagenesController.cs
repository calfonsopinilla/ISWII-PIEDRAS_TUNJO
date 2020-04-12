using Logica;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Utilitarios;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("images")]
    public class ImagenesController : ApiController
    {

        /*
         Autor: Jhonattan Pulido
         Descripción: Servicio para almacenar imagen del servidor y actualizar datos
         Parametros: Int id - Id del usuario
        */

        [HttpPost]
        [Authorize]
        [Route("dniImage")]
        public HttpResponseMessage DniImage([FromUri] int id) {

            string dniName = new LImagen().UploadDni(HttpContext.Current.Request, "Usuarios/Identificaciones");

            if (dniName != null) {

                UUsuario usuario = new UUsuario();
                usuario = new LUsuario().Buscar(id);
                usuario.Imagen_documento = dniName;
                bool actualizado = new LUsuario().Actualizar(id, usuario);

                if (actualizado)
                    return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "DATOS ACTUALIZADOS: Imágen subida correctamente"});
                else
                    return Request.CreateResponse(HttpStatusCode.Conflict, new { ok = false, message = "Error: No se ha podido actualizar el usuario" });


            } else
                return Request.CreateResponse(HttpStatusCode.Conflict, new { ok = false, message = "Error: No se pudo subir la imagen al servidor" });            
        }

        /*
         Autor: Steven Cruz
         Desc: Servicio para almacenar imágenes en el servidor
         Params: el tipo de imagen, ya sea de evento, informacion, identificaciones, etc
             */
        [HttpPost]
        [Route("uploadImage")]
        // POST: images/uploadImage?tipo=identficacion/evento/info/etc...
        public HttpResponseMessage UploadImage([FromUri] string tipo)
        {
            string carpeta = ObtenerCarpetaPorTipo(tipo);
            object objectResponse = new LImagen().UploadImages(HttpContext.Current.Request, carpeta);

            return Request.CreateResponse(objectResponse);
        }

        /*
            Autor: Steven Cruz
            Desc: Servicio para retornar imágenes guardadas en el servidor
            Params:
                - tipo: para saber en que carpeta de imágenes buscar
                - nombre: nombre de la imagen a buscar
            Retorna: La imagen almacenada en el servidor
        */
        [HttpGet]
        [Route("getImage")]
        // GET: /images/getImage?tipo=evento&nombre=image.jpg
        // GET: /images/getImage?tipo=info&nombre=image.jpg
        public HttpResponseMessage GetImage([FromUri] string tipo, [FromUri] string nombre)
        {
            if (string.IsNullOrEmpty(tipo) || string.IsNullOrEmpty(nombre))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            string carpeta = ObtenerCarpetaPorTipo(tipo);

            if (carpeta == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tipo de imagen no encontrado");
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            var path = $"~/Imagenes/{ carpeta }/{ nombre }";
            if (!File.Exists(HttpContext.Current.Server.MapPath(path)))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Imagen no encontrada");
            }
            path = System.Web.Hosting.HostingEnvironment.MapPath(path);
            var ext = Path.GetExtension(path);
            ext = ext.Substring(1); // para que elimine el punto (.) de la extensión
            var contents = File.ReadAllBytes(path);

            MemoryStream ms = new MemoryStream(contents);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue($"image/{ext}");
            return response;
        }

        // Agregar tipos de imagenes almacenadas
        private string ObtenerCarpetaPorTipo(string tipo)
        {
            switch (tipo)
            {
                case "info":
                    return "Informacion";
                case "evento":
                    return "Eventos";
                case "identificacion":
                    return "Usuarios/Identificaciones";
                case "cabana":
                    return "Cabanas";
                case "avatar":
                    return "Usuarios/Avatars";
                case "reserva-tickets":
                    return "Reserva/Tickets";
                case "noticias":
                    return "Noticias";
            }
            return null;
        }
    }
}
