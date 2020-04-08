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
        private UPictograma pictograma;

        public string RegistroPictograma(UPictograma jsonRegistroPic)
        {
            try
            {
             //   UPictograma datosPic = JsonConvert.DeserializeObject<UPictograma>(jsonRegistroPic);


                string existencia = new DAOPictograma().Valida_ExistenciaPictograma(jsonRegistroPic.Id_parque, jsonRegistroPic.Nombre);



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
                            jsonRegistroPic.Calificacion = 0;
                            jsonRegistroPic.Estado = 1;
                            new DAOPictograma().RegistroPictograma(jsonRegistroPic);
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
        public UPictograma LeerPictograma(int pictogramaId)
        {

            try
            {
                return new DAOPictograma().LeerPictograma(pictogramaId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public bool Actualizar(int id, UPictograma pictograma)
        {
            pictograma.Estado = 1;
            return new DAOPictograma().Actualizar(id, pictograma);
        }

        /**
        * Autor: Mary Zapata
        * fecha: 19/03/2019
        * Return: string json con lista de tipo UPictogramas que contiene
        * todos los pictogramas 
       **/
        public List<UPictograma> Mostrar_Pictogramas(int estadoFiltro)
        {
            try
            {
                List <UPictograma> lista = (List<UPictograma>)new DAOPictograma().Mostrar_Pictograma(estadoFiltro);
                List<UComentarioPic> listaCalificaciones = new DAOPictograma().BuscarCalificaciones();
                if (listaCalificaciones != null)
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        List<UComentarioPic> listaFiltro = listaCalificaciones.Where(x => x.PictogramaId == lista[i].Id).ToList();
                        double operacion = 0;
                        for (int j = 0; j < listaFiltro.Count; j++)
                        {
                            operacion = operacion + listaFiltro[j].Calificacionpic;

                        }
                        if (listaFiltro.Count != 0)
                            lista[i].Calificacion = operacion / listaFiltro.Count;
                        else
                            lista[i].Calificacion = 0;
                    }
                }
                else
                {
                    
                }
                



                return lista;
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
        public string CambiarEstado_Pictograma(int id_pictograma)
        {
            try
            {
                string validacion = "";
              //  UPictograma infoCambiar = JsonConvert.DeserializeObject<UPictograma>(json_Info);

              
                    new DAOPictograma().CambiarEstado_Pictograma(id_pictograma);
                    validacion = "Pictograma Eliminado Satisfactoriamente";
                
                return validacion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
