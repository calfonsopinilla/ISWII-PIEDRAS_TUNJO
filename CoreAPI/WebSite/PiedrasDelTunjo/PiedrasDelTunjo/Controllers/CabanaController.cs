using Logica;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Utilitarios;

namespace PiedrasDelTunjo.Controllers
{

    /*
        Autor: Jhonattan Alejandro Pulido Arenas
        Fecha creación: 18/03/2020
        Descripción: Controlador que sirve para hacer CRUD de cabañas
    */
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("cabana")]
    [Authorize]
    public class CabanaController : ApiController
    {
        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer una cabaña filtrada por el nombre
            Recibe: String cabanaNombre - Nombre de la cabaña que se desea traer datos
            Retorna: Objeto de tipo cabaña
        */
        [HttpPost]
        //[Authorize]
        [Route("")]
        public HttpResponseMessage Agregar([FromBody] UCabana cabana)
        {
            if (cabana == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bad request");
            }

            cabana.Token = "";
            bool creado = new LCabana().Agregar(cabana);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = creado });
        }
        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer una cabaña filtrada por el nombre
            Recibe: String cabanaNombre - Nombre de la cabaña que se desea traer datos
            Retorna: Objeto de tipo cabaña
        */
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var cabana = new LCabana().LeerCabana(id);
            if (cabana == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Cabana Not Found" });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, cabana });
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer una cabaña filtrada por el nombre
            Recibe: String cabanaNombre - Nombre de la cabaña que se desea traer datos
            Retorna: Objeto de tipo cabaña
        */
        [HttpGet]
        [Route("leer_nombre")]
        public UCabana LeerCabanaNombre(string cabanaNombre)
        {

            try
            {
                return new LCabana().LeerCabanaNombre(cabanaNombre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer las cabañas existentes alojadas en la base de datos
            Recibe: Nada
            Retorna: Lista de cabañas
        */
        [HttpGet]
        [Route("")]
        public IEnumerable<UCabana> LeerCabanas()
        {

            try
            {
                return new LCabana().LeerCabanas();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer una cabaña filtrada por el nombre
            Recibe: String cabanaNombre - Nombre de la cabaña que se desea traer datos
            Retorna: Objeto de tipo cabaña
        */
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Eliminar([FromUri] int id)
        {
            var cabana = new LCabana().LeerCabana(id);
            if (cabana == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cabana no encontrado");
            }
            var eliminado = new LCabana().BorrarCabana(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = eliminado });
        }
        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer una cabaña filtrada por el nombre
            Recibe: String cabanaNombre - Nombre de la cabaña que se desea traer datos
            Retorna: Objeto de tipo cabaña
        */
        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Actualizar([FromUri] int id, [FromBody] UCabana cabana)
        {
            if (id != cabana.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            cabana.Token = "";
            bool actualizado = new LCabana().Actualizar(id, cabana);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = actualizado });
        }
    }
}
