using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logica;
using Utilitarios;
namespace PiedrasDelTunjo.Controllers
{
    public class AdministradorPqrController : ApiController
    {




        /*
        @Autor : Jose Luis Soriano Roa
        *Fecha de creación: 19/03/2020
        *Descripcion : metodo que envia la informacion de los pqr
        *Este metodo recibe : No resive parametros
        * Retorna: lista de la informacion delos pqr un objeto de tipo UPqrInformacion 
        */



        [HttpGet]
        [Route("administrador/informacionPqr")]

        public IHttpActionResult EnviarInformacion()
        {
            try
            {
                var informacion = new LPqr().informacionPqr();

                return Ok(informacion);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
          @Autor : Jose Luis Soriano Roa
          *Fecha de creación: 19/03/2020
          *Descripcion : metodo que modifica el estado del pqr
          *Este metodo recibe : resive el id del registro 
          * Retorna:True si el registro fue modificado con exito, false si el registro ya esta inacticvo 
          */

        [HttpGet]
        [Route("administrador/cambiarEstadoPqr")]

        public IHttpActionResult cambiarEstado(string id)
        {
            try
            {
                var informacion = new LPqr().cambiarEstado(id);
                return Ok(informacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /*
         @Autor : Jose Luis Soriano Roa
         *Fecha de creación: 19/03/2020
         *Descripcion : metodo que inserta un pqr
         *Este metodo recibe : resive objeto de tipo Upqr 
         * Retorna:True si el registro fue insertado con exito, false si el registro no fue insertado
         */

        [HttpGet]
        [Route("administrador/agregarPqr")]

        public IHttpActionResult agregarPqr(string datosJson)
        {
            try
            {
                var informacion = new LPqr().agregarPqr(datosJson);
                return Ok(informacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        /*
       @Autor : Jose Luis Soriano Roa
       *Fecha de creación: 19/03/2020
       *Descripcion : metodo que modifica pqr-- 
       *Este metodo recibe : resive objeto de tipo upqr en json
       * Retorna: true si el objeto se modifico , false si el registro su estado ya esta en falso
       */

        [HttpGet]
        [Route("administrador/editarPqr")]

        public IHttpActionResult editarPqr(string datosJson)
        {
            try
            {
                var informacion = new LPqr().editarPqr(datosJson);
                return Ok(informacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
