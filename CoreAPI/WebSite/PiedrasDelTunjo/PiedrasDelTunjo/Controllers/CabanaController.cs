using Logica;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Utilitarios;

namespace PiedrasDelTunjo.Controllers {

    /*
        Autor: Jhonattan Alejandro Pulido Arenas
        Fecha creación: 18/03/2020
        Descripción: Controlador que sirve para hacer CRUD de cabañas
    */
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("cabana")]
    [Authorize]
    public class CabanaController : ApiController {
       
        // Variables
        private string imageName;
        private string filePath;
        private List<string> listaUrls;        

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para añadir una nueva cabaña a la base de datos
            Recibe: String datosJson - JSON de tipo cabaña con las especificaciones ya validadas
            Retorna: String con mensaje de confirmación o de error
        */
        [HttpGet]
        [Route("crear")]
        public string CrearCabana(string datosJson, [FromUri] string tipo) {            

            try {

                this.imageName = null;
                this.filePath = null;
                this.listaUrls = new List<string>();

                // Guardar la imagen en el servidor
                var postedFile = HttpContext.Current.Request.Files["image"];
                imageName = postedFile.FileName;                
                filePath = HttpContext.Current.Server.MapPath($"~/Imagenes/Cabana/{ imageName }");
                postedFile.SaveAs(filePath);
                this.listaUrls.Add(this.filePath);

                return new LCabana().CrearCabana(datosJson, this.listaUrls);

            } catch (Exception ex) {
                throw ex;
            }
        }


        [HttpPost]
        [Route("")]
        // POST: 
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

        [HttpGet]
        [Route("{id}")]
        // GET: 
        public HttpResponseMessage Buscar([FromUri] int id)
        {
            var cabana = new LCabana().LeerCabana(id);
            if (cabana == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "Cabana Not Found"  });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, cabana });
        }


        [HttpGet]
        [Route("leer_nombre")]
        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer una cabaña filtrada por el nombre
            Recibe: String cabanaNombre - Nombre de la cabaña que se desea traer datos
            Retorna: Objeto de tipo cabaña
        */
        public UCabana LeerCabanaNombre(string cabanaNombre) {

            try {
                return new LCabana().LeerCabanaNombre(cabanaNombre);
            } catch (Exception ex) {
                throw ex;
            }
        }

        [HttpGet]
        [Route("")]
        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer las cabañas existentes alojadas en la base de datos
            Recibe: Nada
            Retorna: Lista de cabañas
        */
        public IEnumerable<UCabana> LeerCabanas() {

            try {
                return new LCabana().LeerCabanas();
            } catch (Exception ex) {
                throw ex;
            }
        }

        [HttpGet]
        [Route("borrar")]
        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para borrar una cabaña de la base de datos
            Recibe: Integer cabanaId - El id de la cabaña que se desea eliminar
            Retorna: Booleano true
        */
        public bool BorrarCabana(int cabanaId) {

            try {
                return new LCabana().BorrarCabana(cabanaId);
            } catch (Exception ex) {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("{id}")]
        // DELETE: 
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
        [HttpGet]
        [Route("actualizar")]
        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para actuaizar una cabaña de la base de datos
            Recibe: UCabana cabana - Objeto que contiene los datos que se quieren modificar
            Retorna: Booleano true
        */
        public string ActualizarCabana(string datosJson, [FromUri] string tipo) {

            try {

                this.imageName = null;
                this.filePath = null;
                this.listaUrls = new List<string>();

                // Guardar la imagen en el servidor
                var postedFile = HttpContext.Current.Request.Files["image"];
                imageName = postedFile.FileName;                
                filePath = HttpContext.Current.Server.MapPath($"~/Imagenes/Cabana/{ imageName }");
                postedFile.SaveAs(filePath);
                this.listaUrls.Add(this.filePath);

                return new LCabana().ActualizarCabana(datosJson, this.listaUrls);

            } catch (Exception ex) {
                throw ex;
            }
        }


        [HttpPut]
        [Route("{id}")]
        // PUT: 
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
