﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
         Return: object { error, status, path }
             */
        [HttpPost]
        [Route("uploadImage")]
        // POST: images/uploadImage?tipo=identficacion/evento/info/etc...
        public async Task<HttpResponseMessage> UploadImage([FromUri] string tipo)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                Stream reqStream = Request.Content.ReadAsStreamAsync().Result;
                MemoryStream tempStream = new MemoryStream();
                reqStream.CopyTo(tempStream);

                tempStream.Seek(0, SeekOrigin.End);
                StreamWriter writer = new StreamWriter(tempStream);
                writer.WriteLine();
                writer.Flush();
                tempStream.Position = 0;

                StreamContent streamContent = new StreamContent(tempStream);
                foreach (var header in Request.Content.Headers)
                {
                    streamContent.Headers.Add(header.Key, header.Value);
                }

                // Read the form data and return an async task.
                await streamContent.ReadAsMultipartAsync(provider);
                //await Request.Content.ReadAsMultipartAsync(provider);

                foreach(var file in provider.FileData)
                {
                    //FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    //sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
                    string fileName = ClearFileName( file.Headers.ContentDisposition.FileName );
                    string carpeta = ObtenerCarpetaPorTipo(tipo);
                    string path = HttpContext.Current.Server.MapPath($"~/Imagenes/{ carpeta }/{ fileName }");
                    File.Move(file.LocalFileName, path);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Upload Image" });
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        private string ClearFileName(string fileName)
        {
            if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
            {
                fileName = fileName.Trim('"');
            }
            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
            {
                fileName = Path.GetFileName(fileName);
            }
            return fileName;
        }

        //public HttpResponseMessage UploadImage([FromUri] string tipo)
        //{
        //    string carpeta = ObtenerCarpetaPorTipo(tipo);
        //    if (string.IsNullOrEmpty(carpeta))
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tipo de imagen no valido");
        //    }

        //    try
        //    {
        //        var request = HttpContext.Current.Request;
        //        if (request.Files.Count > 0)
        //        {
        //            var postedFile = request.Files.Get("image");
        //            var title = request.Params["title"];
        //            string root = HttpContext.Current.Server.MapPath($"~/Imagenes/{ carpeta }/{ postedFile.FileName }");
        //            postedFile.SaveAs(root);
        //            //Save post to DB
        //            return Request.CreateResponse(HttpStatusCode.OK, new
        //            {
        //                error = false,
        //                status = "created",
        //                path = root
        //            });

        //        } else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No has enviado ninguna imagen");
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


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
                case "cabana":
                    return "Cabana";
            }
            return null;
        }
    }
}
