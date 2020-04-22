// Librerías
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Utilitarios;

/* 
    * Autor: Jhonattan Pulido
    * Descripción: Capa que contiene las operaciones que se pueden realizar con las promociones
    * Feha de modificación: 15-04-2020
*/
namespace Data {

    public class DaoPromocion {

        // Variables
        private readonly Mapeo db = new Mapeo();
        private UPromocion promocion;
        private List<UPromocion> listaPromociones;

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para crear promociones
            * Fecha de modificación: 15-04-2020
            * Parámetros: UPromocion nuevaPromocion - Objeto con los datos de la promoción
            * Retorna: True si la promoción se almaceno correctamente - False si ocurrio un error en la ejecución del método
        */
        public bool CrearPromocion(UPromocion nuevaPromocion) { 

            try {

                using (this.db) {

                    this.db.promocion.Add(nuevaPromocion);
                    this.db.SaveChanges();
                    return true;
                }

            } catch (Exception ex) { return false; }
        }

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para leer todas las promociones
            * Fecha de modificación: 15-04-2020
            * Parámetros: string estado - "1" si se quieren obtener las promociones habilitadas, "2" si se quieren obtener las promociones deshabilitadas
            * Retorna: Lista tipo UPromocion con las promociones - Null si ocurrio un error en la ejecución del método
        */
        public List<UPromocion> LeerPromociones(string estado) {

            try {

                using (this.db) {

                    this.listaPromociones = this.db.promocion.Where(x => x.Estado.Equals(estado)).OrderBy(x => x.Id).ToList();
                    return this.listaPromociones;
                }

            } catch (Exception ex) { return null; }
        }


        
        public List<UPromocion> ObtenerPromociones()
        {
            try
            {
                using (this.db)
                {
                    return this.db.promocion.OrderBy(x => x.Id).ToList();   
                }

            }
            catch (Exception ex) { return null; }
        }


        /*
            * Autor: Jhonattan Pulido
            * Descripción: Método que sirve para cambiar el estado de una promoción
            * Fecha de modificación: 15-04-2020
            * Parámetros: Int id - Identificador de la promoción
            * Retorna: True si el cambio de estado se efectuó correctamente - False si ocurrio un error en la ejecución del método
        */
        public bool CambiarEstado(int id) {

            try {

                using(this.db) {

                    this.promocion = this.db.promocion.Where(x => x.Id == id).FirstOrDefault();
                    if (this.promocion != null) { 

                        if (this.promocion.Estado.Equals("1"))
                            this.promocion.Estado = "2";
                        else
                            this.promocion.Estado = "1";

                        this.promocion.LastModification = DateTime.Now;
                        this.db.Entry(this.promocion).State = EntityState.Modified;
                        this.db.SaveChanges();
                        return true;                        

                    } else
                        return false;
                }

            } catch (Exception ex) { return false; }
        }

        /*
            * Autor: Mary Zapata
            * Descripción: Método que sirve para actualizar promociones
            * Fecha de modificación: 15-04-2020
            * Parámetros: UPromocion nuevaPromocion - Objeto con los datos de la promoción, Int id - Identificador de la promoción
            * Retorna: True si la promoción se actualizo correctamente - False si ocurrio un error en la ejecución del método
        */
        public bool Actualizar(int id, UPromocion promocion) {

            try {

                db.Entry(promocion).State = EntityState.Modified;
                db.SaveChanges();
                return true;

            } catch (DbUpdateConcurrencyException) {

                if (!Existe(id))
                    return false;                
                else               
                    throw;               
            }
        }

        public bool Existe(int id) {
            return db.promocion.Any(x => x.Id == id);
        }

        /*public IEnumerable<UPromocion> ObtenerPromociones() {
            List<UPromocion> promocion = null;
            try
            {
                promocion = db.promocion
                            .OrderBy(x => x.Id)
                            .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return promocion;
        }

        /*public UPromocion Buscar(int id)
        {
            try
            {
                return db.promocion.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/

        /*public bool Agregar(UPromocion promocion)
        {
            try
            {
                db.promocion.Add(promocion);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } */

        /*public bool Eliminar(int id)
        {
            try
            {
                var promocion = db.promocion.Find(id);
                db.promocion.Remove(promocion);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/
    }
}
