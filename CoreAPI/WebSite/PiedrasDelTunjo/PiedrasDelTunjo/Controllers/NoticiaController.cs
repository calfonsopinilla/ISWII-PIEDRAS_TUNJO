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
                var noticias = JsonConvert.DeserializeObject<List<UNoticia>>(new LNoticia().informacionNoticia());
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
        [HttpGet]
        [Route("agregarNoticia")]
        public bool agregarNoticia(string datosJson)
        {
            try
            {
                LNoticia noticia = new LNoticia();
                return noticia.agregarNoticia(datosJson);
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
        public bool eliminarNoticia(int id)
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
