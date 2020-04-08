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
        /// Resive un objeto de tipo ucomentarioNoticia
        /// Si fue o no creada la notica


        [HttpPost]
        [Route("")]
        // POST: comentariosNoticia/
        public HttpResponseMessage AgregarComentarioNotica([FromBody]  UComentarioNoticia  comentarioNoticia)
        {
            if (comentarioNoticia == null){

                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Noticia null" });
            }
            comentarioNoticia.FechaPublicacion = DateTime.Now;
            comentarioNoticia.Token = "asd";
            bool respuesta = new LComentarioNoticias().agregarComentarioNoticia(comentarioNoticia);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = respuesta });
        }


        /// Jose luis Soriano roa 
        /// Fecha : 27/03/2020
        /// No resive parametros
        /// Lista de las noticias con la lista de los comentarios pertenencientes a ellas 


        [HttpGet]
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
        [Route("buscarNoticia1")]
        public IHttpActionResult verNoticia1(int id)
        {

            var noticiasComentarios = new LComentarioNoticias().enviarVerNoticia(id);
            return Ok(noticiasComentarios);
        }

    }
}
