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
    public class AdministradorUsuarioController : ApiController{


        /*
        @Autor : Jose Luis Soriano Roa
        *Fecha de creación: 11/03/2020
        *Descripcion : Servicio que envia la informacion de los usuarios del sistema
        *Resive parametro de tipo UBusqueda en json con rolId y cedula de estructura de utilitario UBUSQUEDA
        *Retorna: lista de la informacion de los usuarios registrados en formato json de tipo UUsuario
        */


        [HttpGet]
       // [Route("administrador/informacionUsuarios")]
        [Route("")]

        public IHttpActionResult EnviarInformacion()
        {
            try
            {
                var informacion = new LAdministradorUsuario().informacionUsuarios();
                return Ok(informacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*
         * 
     @Autor : Jose Luis Soriano Roa
     *Fecha de creación: 11/03/2020
     *Descripcion : Servicio que recibe el registro de un usuario 
     *Recibe: objeto de tipo UUsuario 
     *Retorna: devuelve 1 cuando el usuario fue registrado con exito ,2  reporta que el correo que se esta ingresando ya esta en uso
      3 cuando la cedula que se esta ingresando ya esta registrada y cuatro cuando la cedula y a la vez el correo ya estan registrados 
     
     */
        
        /*   [HttpGet]
           [Route("administrador/agregarUsuario")]

           public string agregareUsuarios(string datosJson)
           {
               try
               {
                   LAdministradorUsuario usuario = new LAdministradorUsuario();
                    return usuario.agregarUsuarioo(datosJson);
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }*/
           

        [HttpPost]
        
        [Route("")]
        // POST: Usuarios/
        public HttpResponseMessage agregarUsuario([FromBody] UUsuario Usuario)
        {
            if (Usuario == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
            }


           Usuario.Token = "";
            bool creado = new LAdministradorUsuario().agregarUsuario(Usuario);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = creado });
        }

        [HttpGet]
        [Route("{id}")]
        // GET: Usuarios/id
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var usuario = new LAdministradorUsuario().Buscar(id);
            if (usuario == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Usuario no encontrado");
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
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            usuario.Token = "";
            bool actualizado = new LAdministradorUsuario().Actualizar(id, usuario);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }

        [HttpDelete]
        [Route("{id}")]
        // DELETE: Usuarios/id
        public HttpResponseMessage Eliminar([FromUri] int id)
        {
            var usuario = new LAdministradorUsuario().Buscar(id);
            if (usuario == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,  "Usuario no encontrado");
            }
            var eliminado = new LAdministradorUsuario().Eliminar(id);
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
                LAdministradorUsuario usuario = new LAdministradorUsuario();
                return usuario.cambiarEstado(cedula);
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /*
        @Autor: Carlos Alfonso Pinilla Garzón
        *Fecha de creación: 13/03/2020
        *Descripcion: Servicio que cambia el estado de cuenta del usuario
        *Recibe: El objeto de tipo UUsuario en un json
        *Retorna: 
        */
        [HttpGet]
        [Route("administrador/actualizarUsuario")]
        public string actualizarUsuario(string datosJson)
        {
            try
            {
                LAdministradorUsuario usuario = new LAdministradorUsuario();
                return usuario.actualizarUsuario(datosJson);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
