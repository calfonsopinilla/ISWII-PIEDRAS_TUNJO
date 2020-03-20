﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utilitarios;
using Logica;


namespace PiedrasDelTunjo.Controllers
{   
    /**
         * Autor: Gabriel Zapata
         * fecha: 18/03/2019
         * Desc: Controller para el CRUD de subscripciones
         * 
         */
    //
    public class SubscripcionController : ApiController
    {
        /**
         * Autor: Gabriel Zapata
         * fecha: 18/03/2019
         * Parametro de recepcion: json tipo USubscripcion, debe traer estado 1 para su registro
         * Return: string estado registro de la subscripcion
         **/

        [HttpGet]
        [Route("Subscripcion/Registro")]
        public string RegistroSubscripcion(string jsonRegistroSub)
        {
            try
            {                
                return new LSubscripcion().RegistroSubscripcion(jsonRegistroSub);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        /**
         * Autor: Gabriel Zapata
         * fecha: 19/03/2019              
         * Return: string json que contiene una lista con todos los registros de subscripciones
         *  traidos de la base de datos
         **/
        [HttpGet]
        [Route("Subscripcion/Ver_Subscripciones")]
        public string MostrarSubscripciones(int estadoFiltro)
        {
            try
            {

                return new LSubscripcion().Mostrar_Subscripciones( estadoFiltro);

            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /**
         * Autor: Gabriel Zapata
         * fecha: 19/03/2019              
         * Return: string que indica un mensaje con respecto al estado del procedimiento de edicion
         **/
        [HttpGet]
        [Route("Subscripcion/Editar_Subscripciones")]
        public string Editar_Subscripciones(string json_InfoNueva)
        {
            try
            {
                return new LSubscripcion().EditarSubscripcion(json_InfoNueva);
                    
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /**
         * Autor: Gabriel Zapata
         * fecha: 19/03/2019  
         * METODO para cambiar estado de la subscripcion y "eliminarla"
         * Return: string que indica un mensaje con respecto al estado del procedimiento de cambio de estado
         **/
        [HttpGet]
        [Route("Subscripcion/Remover_Subscripciones")]
        public string Remover_Subscripciones(string json_Info)
        {
            try
            {
                return new LSubscripcion().CambiarEstado_Subscripcion(json_Info);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
