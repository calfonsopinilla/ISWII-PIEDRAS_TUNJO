using Logica;
using System;
using System.Web.Http;

namespace PiedrasDelTunjo.Controllers {

    /*
        Autor: Jhonattan Alejandro Pulido Arenas
        Fecha creación: 11/03/2020
        Descripción: Clase que contiene los servicios de generar token para validar correo electronico
    */
    public class TokenCorreoController : ApiController {

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 11/03/2020
            Descripción: Método que funciona para generar el token para validar el correo electronico del nuevo usuario que se desea registrar
            Recibe: String datosJson - Datos del nuevo usuario, Int aplicacionId - Id para identificar desde donde se esta solicitando el servicio
            Retorna: Boolean
        */
        [HttpGet]
        [Route("usuario/registro/generar_token")]
        public bool EnviarTokenCorreo(String datosJson, int aplicacionId) {
            return new LTokenCorreo().GenerarToken(datosJson, aplicacionId);
        }
    }
}
