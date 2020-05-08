using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("roles")]
    public class RolesController : ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage ObtenerRoles()
        {
            var roles = new LRol().ObtenerRoles();
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, roles });
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var rol = new LRol().Buscar(id);
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, rol });
        }
    }
}
