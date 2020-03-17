using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilitarios;
using Logica;
namespace PiedrasDelTunjo.Controllers
{
    public class AdministradorUsuarioController : ApiController{


        /*
        @Autor : Jose Luis Soriano Roa
        *Fecha de creación: 11/03/2020
        *Descripcion : Servicio que envia la informacion de los usuarios del sistema
        *Resive parametro de tipo UBusqueda en json con rolId y cedula de estructura de utilitario UBUSQUEDA
        *Retorna: lista de la informacion de los usuarios registrados en formato json de tipo UUsuario
        */


        [HttpGet]
        [Route("administrador/informacionUsuarios")]

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
     *Descripcion : Servicio que resive el registro de un usuario 
     *Resive: objeto de tipo UUsuario 
     *Retorna: devuelve 1 cuando el usuario fue registrado con exito ,2  reporta que el correo que se esta ingresando ya esta en uso
      3 cuando la cedula que se esta ingresando ya esta registrada y cuatro cuando la cedula y a la vez el correo ya estan registrados 
     
     */

        [HttpGet]
        [Route("administrador/agregarUsuario")]

        public string agregarUsuarios(string datosJson)
        {
            try
            {
                LAdministradorUsuario usuario = new LAdministradorUsuario();
                 return usuario.agregarUsuario(datosJson);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        public void actualizarUsuario(string datosJson)
        {
            try
            {
                LAdministradorUsuario usuario = new LAdministradorUsuario();
                usuario.actualizarUsuario(datosJson);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
