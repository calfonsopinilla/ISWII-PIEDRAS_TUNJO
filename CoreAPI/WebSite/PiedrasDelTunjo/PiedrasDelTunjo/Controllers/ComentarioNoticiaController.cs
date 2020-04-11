using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Utilitarios;
using Logica;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("comentariosNoticia")]
    public class ComentarioNoticiaController : ApiController
    {

        

        /// Jose luis Soriano roa 
        /// Fecha : 27/03/2020
        /// No resive parametros
        /// Lista de las noticias con la lista de los comentarios pertenencientes a ellas 

        [HttpGet]
        [AllowAnonymous]
        [Route("noticias")]
        //probado 
        // GET: comentariosNoticia/noticias
        public IHttpActionResult ObtenerListaComentariosNoticia()
        {
            var noticiasComentarios = new LComentarioNoticias().enviarNoticiaComentarios();
            return Ok(noticiasComentarios);
        }

        /// Jose luis Soriano roa 
        /// Fecha : 27/03/2020
        /// id de la noticia
        /// la informacion de la noticia buscada

        [HttpGet]
        [AllowAnonymous]
        [Route("buscarNoticia")]
        public HttpResponseMessage verNoticia([FromUri] int id)
        {
            var noticiasComentarios = new LComentarioNoticias().enviarVerNoticia(id);

            if (noticiasComentarios == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Evento no encontrado" });
            }
            return Request.CreateResponse(HttpStatusCode.OK, noticiasComentarios);

        }

        /// Jose luis Soriano roa 
        /// Fecha : 27/03/2020
        /// id de la noticia no recibido por el body 
        /// la informacion de la noticia buscada

        [HttpGet]
        [AllowAnonymous]
        [Route("buscarNoticia1")]
        public IHttpActionResult verNoticia1(int id)
        {

            var noticiasComentarios = new LComentarioNoticias().enviarVerNoticia(id);
            return Ok(noticiasComentarios);
        }


        [HttpPost]
        [Authorize]
        [Route("")]

        // POST: comentariosNoticia/
        public HttpResponseMessage AgregarComentarioNotica([FromBody] UComentarioNoticia comentarioNoticia)
        {
            if (comentarioNoticia == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Noticia null" });
            }
            comentarioNoticia.FechaPublicacion = DateTime.Now;
            comentarioNoticia.Token = " ";
            bool respuesta = new LComentarioNoticias().agregarComentarioNoticia(comentarioNoticia);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = respuesta });
        }


        /// Jose luis Soriano roa 
        /// Fecha : 08/04/2020
        /// id del comentario a reportar
        /// true si fue exitosa la operacion.

        [HttpPost]
        [Authorize]
        [Route("reportar")]
        public HttpResponseMessage reportarComentario([FromBody] long id)
        {

            bool reportado = new LComentarioNoticias().reportarComentarioNoticia(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = reportado });

        }

        /// Jose luis Soriano roa 
        /// Fecha : 9/04/2020
        /// id del comentario a eliminar
        /// true si fue eliminado falso si no 

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public HttpResponseMessage eliminarComentario([FromUri] long id)
        {

            bool eliminar = new LComentarioNoticias().eliminarComentarioNoticia(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = eliminar });
        }


    }
}
