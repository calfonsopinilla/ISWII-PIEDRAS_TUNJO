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
        [Route("cabana/crear")]
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

        [HttpGet]
        [Route("cabana/leer_id")]
        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer una cabaña filtrada por el Id
            Recibe: Integer cabanaId - ID de la cabaña que se desea traer datos
            Retorna: Objeto de tipo cabaña
        */
        public UCabana LeerCabana(int cabanaId) {

            try {
                return new LCabana().LeerCabana(cabanaId);
            } catch (Exception ex) {
                throw ex;
            }
        }

        [HttpGet]
        [Route("cabana/leer_nombre")]
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
        [Route("cabana/leer")]
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
        [Route("cabana/borrar")]
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

        [HttpGet]
        [Route("cabana/actualizar")]
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
    }
}
