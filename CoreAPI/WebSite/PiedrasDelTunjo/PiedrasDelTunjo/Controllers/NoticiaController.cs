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
    [RoutePrefix("Noticias")]
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
        [Route("noticias")]
        public IHttpActionResult enviarNoticias()
        {
            try
            {
                //var noticias = JsonConvert.DeserializeObject<List<UNoticia>>(new LNoticia().informacionNoticia());
                var noticias = new LNoticia().informacionNoticia();
                return Ok(noticias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("inicio")]
        public IHttpActionResult enviarNoticiasInicio()
        {
            try
            {
                var noticias = new LNoticia().informacionNoticia().OrderByDescending(x => x.Id).Take(3).ToList();
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
                var noticias = new LNoticia().informacionNoticia().OrderByDescending(x => x.Id).Take(3).ToList();
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
        [Route("Agregar")]
        public HttpResponseMessage Agregar([FromBody]UNoticia noticia)
        {
            if (noticia == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Noticia null" });
            }
            bool creado = new LNoticia().agregarNoticia(noticia);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = creado });
        }


        [HttpGet]
        [Route("{id}")]        
        public IHttpActionResult BuscarNoticia([FromUri] int id)
        {                      
            return Ok(new LNoticia().Buscar(id));
        }


        [HttpPut]
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


        /*
       @Autor: Carlos Alfonso Pinilla Garzon
       *Fecha de creación: 18/03/2020
       *Descripcion: 
       *Recibe: 
       *Retorna: 
       */
        [HttpGet]
        [Route("actualizarNoticia")]
        public bool actualizarNoticia(string datosJson)
        {
            try
            {
                LNoticia noticia = new LNoticia();
                return noticia.actualizarNoticia(datosJson);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*
       @Autor: Carlos Alfonso Pinilla Garzon
       *Fecha de creación: 18/03/2020
       *Descripcion: 
       *Recibe: 
       *Retorna: 
       */
        [HttpGet]
        [Route("eliminarNoticia")]
        public bool eliminarNoticia([FromUri]int id)
        {
            try
            {
                LNoticia noticia = new LNoticia();
                return noticia.eliminarNoticia(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
