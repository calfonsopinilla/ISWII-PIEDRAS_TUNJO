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



        [HttpGet]
        [Route("usuario/iniciaSesion")]
        public IHttpActionResult EnviarInformacion(string datosLogin){
            try
            {

                var informacion = new LUsuario().iniciarSesion(datosLogin);
                if (!informacion.Equals("2") && !informacion.Equals("1"))
                {
                    var informacion2 = JsonConvert.DeserializeObject<UUsuario>(informacion);
                    return Ok(informacion2);
                }
                else {
                    return Ok(informacion);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
