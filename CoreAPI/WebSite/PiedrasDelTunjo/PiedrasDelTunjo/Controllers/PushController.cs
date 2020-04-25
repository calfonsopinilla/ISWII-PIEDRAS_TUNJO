﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Logica;
using Utilitarios;
namespace PiedrasDelTunjo.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("push")]

    public class PushController : ApiController {

        [HttpPost]
        [Route("crear")]
        public HttpResponseMessage push([FromBody] UPush push){
            bool creado = new Lpush().insertarPush(push);
            return Request.CreateResponse(HttpStatusCode.Created, new { ok = creado });
        }


        [HttpGet]
        [Route("push")]
        public HttpResponseMessage psuh(){
            //string retured = new Lpush().SendNotification();
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "push sent");

        }
    }


}
