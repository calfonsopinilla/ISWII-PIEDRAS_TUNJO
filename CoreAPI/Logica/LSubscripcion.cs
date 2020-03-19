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
    public class LSubscripcion
    {

        /**
           * Autor: Gabriel Zapata
           * fecha: 18/03/2019
           * Parametro de recepcion: json tipo USubscripcion
           * Return: int estado registro de la subscripcion
        **/
        public string RegistroSubscripcion(string jsonRegistroSub)
        {
            try
            {
                USubscripcion datosSub = JsonConvert.DeserializeObject<USubscripcion>(jsonRegistroSub);

                string estado = new DAOSubscripcion().Valida_ExistenciaSubscripcion(datosSub.Subscripcion, datosSub.ValorSubscripcion);
                int validacion = 0;

                switch (estado)
                {
                    case "La subscripcion ya ha sido creada con ese valor":
                        validacion = 1; //existe la subscripcion con el mismo valor
                        break;
                    case "La subscripcion ya ha sido creada con diferente valor":
                        validacion = 2; //existe la subscripcion con diferente valor                
                        break;
                    case "La subscripcion no ha sido creada":
                        validacion = 3; // no existe la validacion
                        break;

                }
                switch (validacion)
                {
                    case 1:
                        return estado;
                    case 2:
                        return estado;
                    case 3:
                        try
                        {
                            new DAOSubscripcion().RegistroSubscripcion(datosSub);
                            estado = "Subscripcion insertada satisfactoriamente";
                        }
                        catch(Exception ex)
                        {
                            throw ex;
                        }                        
                        break;

                }                
                return estado;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }



    }
}
