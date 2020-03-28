﻿using System;
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
           * Return: string estado registro de la subscripcion
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

        /**
         * Autor: Gabriel Zapata
         * fecha: 19/03/2019
         * Return: string json con lista de tipo USubscripcion que contiene
         * todos las subscripciones 
        **/
        public string Mostrar_Subscripciones(int estadoFiltro)
        {
            try
            {
                return (JsonConvert.SerializeObject((List<USubscripcion>)new DAOSubscripcion().Mostrar_Subscripciones(estadoFiltro))).ToString();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            

        }

        /**
         * Autor: Gabriel Zapata
         * fecha: 19/03/2019
         * Return: string con mensaje de edicion exitosa
        **/
        public string EditarSubscripcion(string json_InfoNueva)
        {
            try
            {
                string validacionCoincidencia = "";
                //int id_InformacionAnt = int.Parse(id_InfoAnterior);
                USubscripcion infoNueva = JsonConvert.DeserializeObject<USubscripcion>(json_InfoNueva);

                //string validacionCoincidencia = new DAOSubscripcion().Valida_CoincidenciaEdicion(id_InformacionAnt, infoNueva);
                if (infoNueva == null)
                {
                    validacionCoincidencia = "Los campos estan vacios";
                    return validacionCoincidencia;
                }
                else
                {
                    new DAOSubscripcion().EditarSubscripcion(infoNueva);
                    validacionCoincidencia = "Subscripcion Editada Satisfactoriamente";


                }                
                /*switch (validacionCoincidencia)
                {
                    case "Los datos a modificar no han cambiado ó intenta modificar una subscripcion que ya esta registrada en otro registro":
                        return validacionCoincidencia;                    
                    case "Se puede editar":
                       // new DAOSubscripcion().EditarSubscripcion(infoNueva);
                       // validacionCoincidencia = "Subscripcion Editada Satisfactoriamente";
                        break;
                }*/

                return validacionCoincidencia;


            }catch(Exception ex)
            {
                throw ex;
                
            }


        }
        /**
         * Autor: Gabriel Zapata
         * fecha: 19/03/2019
         * 
         * Return: string con mensaje del cambio de estado
        **/
        public string CambiarEstado_Subscripcion(string json_Info)
        {
            try
            {
                string validacion = "";                
                USubscripcion infoCambiar = JsonConvert.DeserializeObject<USubscripcion>(json_Info);

                if (infoCambiar == null)
                {
                    validacion = "Los campos estan vacios";
                    return validacion;
                }
                else
                {
                    new DAOSubscripcion().CambiarEstado_Subscripcion(infoCambiar);
                    validacion = "Subscripcion Editada Satisfactoriamente";


                }


                return validacion;

            }catch(Exception ex)
            {
                throw ex;
            }
        }


    }
}
