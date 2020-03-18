﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PiedrasDelTunjo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("images")]
    public class ImagenesController : ApiController
    {
        /*
         Autor: Steven Cruz
         Desc: Servicio para almacenar imágenes en el servidor
         Params: el tipo de imagen, ya sea de evento, informacion, identificaciones, etc
         Return: 200 OK Status
             */
        [HttpPost]
        [Route("uploadImage")]
        public HttpResponseMessage UploadImage([FromUri] string tipo)
        {
            string imageName = null;
            string carpeta = ObtenerCarpetaPorTipo(tipo);

            if (carpeta == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tipo de imagen no valido");
            }

            try
            {
                // Guardar la imagen en el servidor
                var postedFile = HttpContext.Current.Request.Files["image"];
                imageName = postedFile.FileName;
                //imageName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                // imageName = $"{ imageName.Replace(" ", "_") }{ DateTime.Now.ToString("yymmssfff") }{ Path.GetExtension(postedFile.FileName) }";
                string filePath = HttpContext.Current.Server.MapPath($"~/Imagenes/{ carpeta }/{ imageName }");
                postedFile.SaveAs(filePath);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        /*
            Autor: Steven Cruz
            Desc: Servicio para retornar imágenes guardadas en el servidor
            Params:
                - tipo: para saber en que carpeta de imágenes buscar
                - nombre: nombre de la imagen a buscar
            Retorna: La imagen almacenada en el servidor
        */
        [HttpGet]
        [Route("getImage")]
        // GET: /images/getImage?tipo=evento&nombre=image.jpg
        // GET: /images/getImage?tipo=info&nombre=image.jpg
        public HttpResponseMessage GetImage([FromUri] string tipo, [FromUri] string nombre)
        {
            if (string.IsNullOrEmpty(tipo) || string.IsNullOrEmpty(nombre))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            string carpeta = ObtenerCarpetaPorTipo(tipo);

            if (carpeta == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tipo de imagen no encontrado");
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            var path = $"~/Imagenes/{ carpeta }/{ nombre }";
            if (!File.Exists(HttpContext.Current.Server.MapPath(path)))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Imagen no encontrada");
            }
            path = System.Web.Hosting.HostingEnvironment.MapPath(path);
            var ext = Path.GetExtension(path);
            ext = ext.Substring(1); // para que elimine el punto (.) de la extensión
            var contents = File.ReadAllBytes(path);

            MemoryStream ms = new MemoryStream(contents);
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue($"image/{ext}");
            return response;
        }

        // Agregar tipos de imagenes almacenadas
        private string ObtenerCarpetaPorTipo(string tipo)
        {
            switch (tipo)
            {
                case "info":
                    return "Informacion";
                case "evento":
                    return "Eventos";
                case "identificacion":
                    return "Usuarios/Identificaciones";
            }
            return null;
        }
    }
}
