using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilitarios;
using Logica;
using System.Web.Http.Cors;
using System.Collections.Generic;

namespace PiedrasDelTunjo.Controllers {

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Usuarios")]
    [Authorize]
    public class UsuarioController : ApiController{

        /*
            * Autor: Jhonattan Pulido
            * Fecha creación: 21/04/2020
            * Descripción: Servicio que funciona para traer los estados que estan registrados pero no han sido verificados por el administrador
            * Parámetros: Ninguno
            * Retorna: Lista de usuarios filtrados por verificación de cuenta en falso
            * Ruta: Usuarios/leer/no-verificados
        */
        [HttpGet]
        //[Authorize]
        [Route("leer/no-verificados")]
        public IHttpActionResult LeerUsuariosNoVerificados() {

            try {
                var informacion = new LUsuario().LeerUsuariosNoVerificados();
                return Ok(informacion);

            } catch (Exception ex)
            {
                throw ex;
            }            
        }

        /*
            * Autor: Jhonattan Pulido
            * * Fecha creación: 21/04/2020
            * Descripción: Servicio que funciona para habilitar al usuario para poder realizar funciones desde movil
            * Parámetros: UUsuario usuario: Objeto con los datos del usuario inscritos
            * Retorna: True: Si la ejecución del servicio se hizo correctamente, False: Si ocurrio un error durante la ejecución del método
            * Ruta: Usuarios/actualizar/no-verificado
        */
        [HttpPut]
        [Route("actualizar/no-verificado")]
        public HttpResponseMessage ActualizarUsuarioNoVerificado([FromBody] UUsuario usuario) {

            usuario.VerificacionCuenta = true;
            bool actualizado = new LUsuario().Actualizar(usuario.Id, usuario);
            if (actualizado)
                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Usuario actualizado correctamente" });
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = true, message = "ERROR: Ha ocurrido un error inesperado" });
        }

        /*
        @Autor : Jose Luis Soriano Roa
        *Fecha de creación: 11/03/2020
        *Descripcion : Servicio que envia la informacion de los usuarios del sistema
        *Recibe parametro de tipo UBusqueda en json con rolId y cedula de estructura de utilitario UBUSQUEDA
        *Retorna: lista de la informacion de los usuarios registrados en formato json de tipo UUsuario
        */

        [HttpGet]
        [Route("")]
        public HttpResponseMessage ObtenerUsuarios()
        {
            try
            {
                var usuarios = new LUsuario().ObtenerUsuarios();
                return Request.CreateResponse(HttpStatusCode.OK, usuarios);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /**
         * @Autor: Gabriel Andres Zapata Morera
         * Fecha de Creacion: 14/04/2020
         * Descripcion: Servicio que retorna los usuarios activos
         */
        [HttpGet]
        [Route("Ver_Usuarios")]
        public IHttpActionResult ObtenerUsuarios_Filtrados([FromUri]int estadoFiltro)
        {
            try
            {
                var informacion = new LUsuario().ObtenerUsuarios_Filtrados(estadoFiltro);
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

        /**
        @Autor: Gabriel Andres Zapata Morera
        *Fecha de creación: 14/04/2020
        *Descripcion: Servicio que cambia el estado de cuenta del usuario
        *Recibe: id_Usuario y estadoFiltro
        *Retorna: IHttpActionResult
        */
        [HttpGet]
        [Route("Estado_Usuario")]
        public IHttpActionResult CambiarEstado_Usuarios([FromUri]int estadoFiltro, [FromUri]int id_Usuario)
        {
            try
            {                
                return Ok(new LUsuario().CambiarEstado_Usuarios(estadoFiltro, id_Usuario));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("RolesUsuario")]
        public IHttpActionResult ObtenerRoles()
        {
            try
            {
                return Ok(new LUsuario().ObteneRoles());
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
