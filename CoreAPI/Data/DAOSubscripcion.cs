using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;


namespace Data
{
    public class DAOSubscripcion
    { /**
         * Autor: Gabriel Zapata
         * Parametro de recepcion: string subscripcion a buscar y double valor a buscar
         * 
         * return string busqueda
         **/
        public string Valida_ExistenciaSubscripcion(string subscripcion, double valor)
        {
            string busqueda = "";

            using (var db = new Mapeo())
            {
                    
                if (db.infoSubscripcion.Where(x => x.Subscripcion == subscripcion ).FirstOrDefault() != null && (db.infoSubscripcion.Where(x => x.ValorSubscripcion == valor).FirstOrDefault() != null))
                {
                     busqueda = "La subscripcion ya ha sido creada con ese valor";

                }else if ((db.infoSubscripcion.Where(x => x.Subscripcion == subscripcion).FirstOrDefault()  != null)&& (db.infoSubscripcion.Where(x => x.ValorSubscripcion != valor).FirstOrDefault() != null))
                {
                     busqueda = "La subscripcion ya ha sido creada con diferente valor";  
                }
                else
                {
                     busqueda = "La subscripcion no ha sido creada";
                }

            }

            return busqueda;
        }


        /**
         * Autor: Gabriel Zapata
         * Metodo para insertar una nueva subscripcion en base de datos en la tabla subscripcion
         * Parametro de recepcion: objeto USubscripcion para insertar en la bd
         * return: void
         **/
        public void RegistroSubscripcion(USubscripcion datosInsertar)
        {

            using (var db = new Mapeo())
            {

                try
                {
                    db.infoSubscripcion.Add(datosInsertar);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }
        }



    }
}
