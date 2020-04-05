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
    [EnableCors(origins: "*", methods: "*", headers: "*")]
    public class CuentaController : ApiController
    {

        /*
              @Autor : Jose Luis Soriano Roa
              *Fecha de creación: 18/03/2020
              *Descripcion : Metodo que trae la informacion del usuario que va a iniciar sesion.
              *Este metodo recibe : Resive un objeto de tipo Ulogin quien incorpora correoElectronico y clave en json 
      */
        [HttpPost]
        [Route("cuenta/iniciaSesion")]
        public HttpResponseMessage IniciarSesion([FromBody] UUsuario usuario){
            try
            {
                var userLogin = new LCuenta().IniciarSesion(usuario.CorreoElectronico, usuario.Clave);
                if (userLogin == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "El usuario no existe" });
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
