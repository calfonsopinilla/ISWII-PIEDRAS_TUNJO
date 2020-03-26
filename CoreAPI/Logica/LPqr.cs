using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using Data;
using Newtonsoft.Json;

namespace Logica
{
    public class LPqr
    {

        /*
        @Autor : Jose Luis Soriano Roa
        *Fecha de creación: 19/03/2020
        *Descripcion : metodo que envia la informacion de los pqr
        *Este metodo recibe : No resive parametros
        * Retorna: lista de la informacion delos pqr un objeto de tipo UPqrInformacion 
        */

        public List<UPqrInformacion> informacionPqr()
        {
            try
            {
                return new DaoPqr().informacionPqr();
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


        public string cambiarEstado(string id)
        {
            try
            {

                UId ids= JsonConvert.DeserializeObject<UId>(id);
                return  new DaoPqr().cambiarEstado(ids.Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /*
        @Autor : Jose Luis Soriano Roa
        *Fecha de creación: 19/03/2020
        *Descripcion : metodo que inserta un pqr-- 
        *Este metodo recibe : Un objeto de tipo UPqr ....
        * Retorna: true si se inserto con exito , false si no fue posible insertar el registro
        */

        public string agregarPqr(string datosJson){
            try{
                UPQR pqr = JsonConvert.DeserializeObject<UPQR>(datosJson);
                new DaoPqr().agregarPqr(pqr);
                return "true";
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

        public string editarPqr(string datosJson)
        {
            try
            {
                UPQR pqr = JsonConvert.DeserializeObject<UPQR>(datosJson);
                
                return new DaoPqr().editarPqr(pqr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
