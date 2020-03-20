using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilitarios;
using Logica;
using Newtonsoft.Json;

namespace PiedrasDelTunjo.Controllers
{
    public class UsuarioController : ApiController
    {


        /*
      @Autor : Jose Luis Soriano Roa
      *Fecha de creación: 18/03/2020
      *Descripcion : Metodo que trae la informacion del usuario que va a iniciar sesion.
      *Este metodo recibe : Resive un objeto de tipo Ulogin quien incorpora correoElectronico y clave en json 
      * Retorna: Si el usuario es encontrado retorna toda la informacion de el, si el usuario no ha realizado
      * la verificacion del correo retorna 1 y si el usuario no esta registrado retorna 2.
      */



        [HttpPost]
        [Route("usuario/iniciaSesion")]
        public HttpResponseMessage IniciarSesion([FromBody] UUsuario usuario){
            try
            {
                var userLogin = new LUsuario().IniciarSesion(usuario.CorreoElectronico, usuario.Clave);
                if (userLogin == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "El Usuario no existe" });
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, userLogin });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
