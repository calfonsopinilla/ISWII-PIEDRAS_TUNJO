using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using Data;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Logica
{
    public class LInformacion
    {

        /*
         @Autor : Jose Luis Soriano Roa
         *Fecha de creación: 11/03/2020
         *Descripcion : Metodo que resive la lista de la informacion del parque .
         *Este metodo recibe : No resive parametros
         * Retorna: lista de la informacion del parque 
         */

        public string informacionParque() {
            try
            {
                List<UInformacionParque> informacion = new DaoInformacion().informacionParque();
                if (informacion.Count() < 0)
                {
                    return "null";
                }
                else
                {
                    for (int x = 0; x < informacion.Count(); x++)
                    {

                        if (!String.IsNullOrEmpty(informacion[x].ImagenesUrl))
                        {
                            informacion[x].ListaImagenesUrl = JsonConvert.DeserializeObject<List<string>>(informacion[x].ImagenesUrl);
                        }
                    }
                    return JsonConvert.SerializeObject(informacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
        @Autor : Jose Luis Soriano Roa
        *Fecha de creación: 11/03/2020
        *Descripcion : Metodo que resive los datos de 
        *Este metodo recibe : No resive parametros
        * Retorna: lista de la informacion del parquee 
        */


        public string informacionParqueInicioWeb(){

            try
            {
                UInformacionParque informacion = new DaoInformacion().informacionInicioWeb();
                if (!informacion.Equals(null))
                {
                    if (!String.IsNullOrEmpty(informacion.ImagenesUrl))
                    {
                        informacion.ListaImagenesUrl = JsonConvert.DeserializeObject<List<string>>(informacion.ImagenesUrl);
                    }
                }
                return JsonConvert.SerializeObject(informacion);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
