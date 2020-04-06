using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Data
{
    public class DAOSubscripcion
    {
        /**
         * Autor: Gabriel Zapata
         * Fecha: 18/03/2020
         * Parametro de recepcion: string subscripcion a buscar y double valor a buscar
         * return string busqueda
         **/
        private Mapeo conexionBD;
        private USubscripcion subscripcion;
        private List<USubscripcion> listaSubscripciones;
        private readonly Mapeo db = new Mapeo();

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
         * Fecha: 18/03/2020
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

        /**
         * Autor: Gabriel Zapata
         * Fecha: 18/03/2020
         * Metodo para consultar las subscripcion en base de datos en la tabla subscripcion         
         * return: List<USubscripcion> listaSubs
         **/
        public List<USubscripcion> Mostrar_Subscripciones(int estadoFiltro)
        {

            using (var db = new Mapeo())
            {
                try
                {
                    return db.infoSubscripcion.Where(x => x.Estado == estadoFiltro).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /**
        * Autor: Gabriel Zapata
        * Fecha: 19/03/2020
        * Metodo para consultar las subscripciones en base de datos en la tabla subscripcion        
        * comparandolos con los datos a editar para encontrar similitudes
        * return: string validacion
        **/
       /* public string Valida_CoincidenciaEdicion(int id_InformacionAnt, USubscripcion infoNueva)
        {
            try
            {

                string validacion = "";
                using (var db = new Mapeo())
                {
                    
                    if (db.infoSubscripcion.Where(x => x.Id_subscripcion == infoNueva.Id_subscripcion).FirstOrDefault() != null)
                    {
                        


                    }
                    if (db.infoSubscripcion.Where(x => x.Subscripcion == infoNueva.Subscripcion).FirstOrDefault() != null
                        && db.infoSubscripcion.Where(x => x.ContenidoSubscripcion == infoNueva.ContenidoSubscripcion).FirstOrDefault() != null
                        && db.infoSubscripcion.Where(x => x.ValorSubscripcion == infoNueva.ValorSubscripcion).FirstOrDefault() != null
                        && db.infoSubscripcion.Where(x => x.Estado == 1).FirstOrDefault() != null
                       //&& db.infoSubscripcion.Where(x => x.Id_subscripcion == id_InformacionAnt).FirstOrDefault() != null
                        )
                    {
                        validacion = "Los datos a modificar no han cambiado ó intenta modificar una subscripcion que ya esta registrada en otro registro";

                    }
                    else if (db.infoSubscripcion.Where(x => x.Subscripcion == infoNueva.Subscripcion).FirstOrDefault() != null
                             && db.infoSubscripcion.Where(x => x.ContenidoSubscripcion != infoNueva.ContenidoSubscripcion).FirstOrDefault() != null
                             && db.infoSubscripcion.Where(x => x.ValorSubscripcion != infoNueva.ValorSubscripcion).FirstOrDefault() != null
                             && db.infoSubscripcion.Where(x => x.Estado == 1).FirstOrDefault() != null
                             && db.infoSubscripcion.Where(x => x.Id_subscripcion != id_InformacionAnt).FirstOrDefault() != null
                             )
                    {
                        validacion = "Se puede editar";
                    }

                    else
                    {
                        validacion = "Se puede editar";

                    }

                }
                return validacion;

            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        */
        /**
        * Autor: Gabriel Zapata
        * Fecha: 19/03/2020
        * Metodo para editar las subscripciones en base de datos en la tabla subscripcion        
        * return: void
        **/
        public void EditarSubscripcion(int id, USubscripcion infoNueva)
        {
            using (var db = new Mapeo())
            {
                USubscripcion edit = db.infoSubscripcion.Where(x => x.Id_subscripcion == id).First();

                edit.ContenidoSubscripcion = infoNueva.ContenidoSubscripcion;
                edit.ValorSubscripcion = infoNueva.ValorSubscripcion;
                edit.Subscripcion = infoNueva.Subscripcion;
                edit.Imagen_Subscripcion = infoNueva.Imagen_Subscripcion;

                db.infoSubscripcion.Attach(edit);
                var entry = db.Entry(edit);
                entry.State = EntityState.Modified;

                db.SaveChanges();



            }

        }
        public USubscripcion BusquedaSubscripcion(int id)
        {
            try
            {

                this.subscripcion = new USubscripcion();

                using (this.conexionBD = new Mapeo())
                {

                    this.subscripcion = this.conexionBD.infoSubscripcion.Find(id);
                    return this.subscripcion;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /**
         * Autor: Gabriel Zapata
         * Fecha: 19/03/2020
         * Metodo para editar el estado de las subscripciones en base de datos en la tabla subscripcion        
         * return: void
         **/
        public void CambiarEstado_Subscripcion(USubscripcion infoCambiar)
        {
            using (var db = new Mapeo())
            {
                var subs = db.infoSubscripcion.Where(x => x.Id_subscripcion == infoCambiar.Id_subscripcion).FirstOrDefault();

                if (subs.Estado == 2)
                {
                    subs.Estado = 1;
                }
                else
                {
                    subs.Estado = 2;
                }
                db.SaveChanges();
            }
        }

        public bool ActualizarSuscripcion(int id, USubscripcion subscripcion)
        {
            try
            {
                db.Entry(subscripcion).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Existe(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public bool Existe(int id)
        {
            return db.infoSubscripcion.Any(x => x.Id_subscripcion == id);
        }


    }
}
