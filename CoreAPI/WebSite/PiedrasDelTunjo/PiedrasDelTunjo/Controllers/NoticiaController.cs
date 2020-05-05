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

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("noticias")]
    public class NoticiaController : ApiController
    {
        /*
       @Autor: Carlos Alfonso Pinilla Garzon
       *Fecha de creación: 18/03/2020
       *Descripcion: Este metodo trae las noticias
       *Recibe: 
       *Retorna: una lista de noticias
       */
        [HttpGet]
        [Route("")]
        public HttpResponseMessage ObtenerNoticias()
        {
            try
            {
                var noticias = new LNoticia().ObtenerNoticias();
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, noticias });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("inicio")]
        public IHttpActionResult ObtenerNoticiasInicio()
        {
            try
            {
                var noticias = new LNoticia().ObtenerNoticias().OrderByDescending(x => x.Id).Take(3).ToList();
                return Ok(noticias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("inicio2")]
        public IHttpActionResult enviarNoticiasInicio2()
        {
            try
            {
                var noticias = new LNoticia().ObtenerNoticias().OrderByDescending(x => x.Id).Take(3).ToList();
                return Ok(noticias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
       @Autor: Carlos Alfonso Pinilla Garzon
       *Fecha de creación: 18/03/2020
       *Descripcion: Agrega las noticias
       *Recibe: una estreuctura json para agregar los datos
       *Retorna: un true para saber que se agregaron los datos
       */
        [HttpPost]
        [Authorize]
        [Route("")]
        public HttpResponseMessage Agregar([FromBody] UNoticia noticia)
        {
            if (noticia == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Noticia null" });
            }
            noticia.FechaPublicacion = DateTime.Now;
            bool creado = new LNoticia().AgregarNoticia(noticia);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = creado });
        }


        [HttpGet]
        [Route("{id}")]        
        public HttpResponseMessage BuscarNoticia([FromUri] int id)
        {
            var noticia = new LNoticia().Buscar(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = (noticia != null), noticia });
        }


        [HttpPut]
        [Authorize]
        [Route("{id}")]   
        public HttpResponseMessage ActualizarNoticia([FromUri] int id, [FromBody] UNoticia noticia)
        {
            if (id != noticia.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Bad Request" });
            }
            bool actualizado = new LNoticia().Actualizar(id, noticia);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public HttpResponseMessage EliminarNoticia([FromUri] int id)
        {
            var noticia = new LNoticia().Buscar(id);
            if (noticia == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = false, message = "bad request" });
            }
            bool deleted = new LNoticia().EliminarNoticia(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = deleted });
        }
    }
}
