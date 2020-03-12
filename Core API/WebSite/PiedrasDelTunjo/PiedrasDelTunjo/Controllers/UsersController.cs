using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PiedrasDelTunjo.Controllers
{
    [RoutePrefix("usuarios")]
    public class UsersController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetUsers()
        {
            return Ok( new LUser().GetUsers() );
        }
    }
}
