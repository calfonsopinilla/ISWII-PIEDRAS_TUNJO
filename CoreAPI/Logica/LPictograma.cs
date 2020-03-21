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
    /**
         * Autor: Mary Zapata
         * fecha: 19/03/2019
         * Parametro de recepcion: json tipo UPictograma
         * Return: string estado registro del pictograma
      **/
    public class LPictograma
    {

        public string RegistroPictograma(string jsonRegistroPic)
        {
            try
            {
                UPictograma datosPic = JsonConvert.DeserializeObject<UPictograma>(jsonRegistroPic);

                
                string existencia = new DAOPictograma().Valida_ExistenciaPictograma(datosPic.Id_parque, datosPic.Nombre);



                 switch (existencia)
                 {
                     case "Ya exite un pictograma con el mismo nombre y el mismo id, eliminelo o actualicelo":
                         return existencia;
                     case "Ya exite un pictograma con el mismo id, eliminelo o actualicelo":
                         return existencia;                
                     case "Ya exite un pictograma con el mismo nombre, eliminelo o actualicelo":
                        return existencia;
                    case "El pictograma no ha sido creado":
                        try
                        {
                            new DAOPictograma().RegistroPictograma(datosPic);
                            existencia = "Pictograma insertada satisfactoriamente";
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        break;


                }

               return existencia;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /**
        * Autor: Mary Zapata
        * fecha: 19/03/2019
        * Return: string json con lista de tipo UPictogramas que contiene
        * todos los pictogramas 
       **/
        public string Mostrar_Pictogramas(int estadoFiltro)
        {
            try
            {
                return JsonConvert.SerializeObject((List<UPictograma>)new DAOPictograma().Mostrar_Pictograma(estadoFiltro)).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        /**
         * Autor: Mary Zapata
         * fecha: 20/03/2019
         * Return: string con mensaje de edicion exitosa
        **/

        public string EditarPictograma(string json_InfoNueva)
        {
            try
            {
                string validacionCoincidencia = "";
                UPictograma infoNueva = JsonConvert.DeserializeObject<UPictograma>(json_InfoNueva);

               if (infoNueva == null)
                {
                    validacionCoincidencia = "Los campos estan vacios";
                    return validacionCoincidencia;
                }
                else
                {
                    new DAOPictograma().EditarPictograma(infoNueva);
                    validacionCoincidencia = "Pictograma Editada Satisfactoriamente";


                }
                
                return validacionCoincidencia;
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }

        /**
        * Autor: Mary Zapata
        * fecha: 20/03/2019
        * 
        * Return: string con mensaje del cambio de estado
       **/
        public string CambiarEstado_Pictograma(string json_Info)
        {
            try
            {
                string validacion = "";
                UPictograma infoCambiar = JsonConvert.DeserializeObject<UPictograma>(json_Info);

                if (infoCambiar == null)
                {
                    validacion = "Los campos estan vacios";
                    return validacion;
                }
                else
                {
                    new DAOPictograma().CambiarEstado_Pictograma(infoCambiar);
                    validacion = "Pictograma Eliminado Satisfactoriamente";
                }
                return validacion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
