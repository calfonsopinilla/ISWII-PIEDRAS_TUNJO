using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilitarios;
using Logica;
using System.Web.Http.Cors;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Usuarios")]
    public class UsuarioController : ApiController{

        /*
        @Autor : Jose Luis Soriano Roa
        *Fecha de creación: 11/03/2020
        *Descripcion : Servicio que envia la informacion de los usuarios del sistema
        *Resive parametro de tipo UBusqueda en json con rolId y cedula de estructura de utilitario UBUSQUEDA
        *Retorna: lista de la informacion de los usuarios registrados en formato json de tipo UUsuario
        */

        [HttpGet]
        [Route("")]
        public IHttpActionResult ObtenerUsuarios()
        {
            try
            {
                var informacion = new LUsuario().ObtenerUsuarios();
                return Ok(informacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("")]
        // POST: usuarios/
        public HttpResponseMessage agregarUsuario([FromBody] UUsuario usuario)
        {
            if (usuario == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Usuario null" });
            }

            object respuesta = new LUsuario().Agregar(usuario);
            return Request.CreateResponse(HttpStatusCode.Created, respuesta);
        }

        [HttpGet]
        [Route("{id}")]
        // GET: Usuarios/id
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var usuario = new LUsuario().Buscar(id);
            if (usuario == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Usuario no encontrado" });
            }
            return Request.CreateResponse(HttpStatusCode.OK, usuario);
        }

        [HttpPut]
        [Route("{id}")]
        // PUT: Usuarios/id
        public HttpResponseMessage Actualizar([FromUri] int id, [FromBody] UUsuario usuario)
        {
            if (id != usuario.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "Bad Request" });
            }
            bool actualizado = new LUsuario().Actualizar(id, usuario);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }

        [HttpDelete]
        [Route("{id}")]
        // DELETE: Usuarios/id
        public HttpResponseMessage Eliminar([FromUri] int id)
        {
            var usuario = new LUsuario().Buscar(id);
            if (usuario == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Usuario no encontrado" });
            }
            var eliminado = new LUsuario().Eliminar(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = eliminado });
        }

        /*
        @Autor: Carlos Alfonso Pinilla Garzón
        *Fecha de creación: 13/03/2020
        *Descripcion: Servicio que cambia el estado de cuenta del usuario
        *Recibe: 
        *Retorna: 
        */
        [HttpGet]
        [Route("administrador/cambiarEstado")]
        public bool cambiarEstado(string cedula)
        {
            try
            {
                LUsuario usuario = new LUsuario();
                return usuario.cambiarEstado(cedula);
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
