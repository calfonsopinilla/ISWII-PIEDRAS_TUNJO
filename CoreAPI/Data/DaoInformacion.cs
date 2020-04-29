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

    public class DaoInformacion {

        /*
        @Autor : Jose Luis Soriano Roa
        *Fecha de creación: 11/03/2020
        *Descripcion : Metodo que trae la lista de la informacion del parque de la base de datos
        *Este metodo recibe : No resive parametros
        * Retorna: lista de la informacion del parque
        */

        public List<UInformacionParque> informacionParque() {

            using (var db = new Mapeo()) {
                try {
                    return db.informacionParque
                                .OrderBy(x => x.Id)
                                .ToList();
                }
                catch (  Exception ex ) {
                    throw ex; 
                }
            }
        }


        /*
        @Autor : Jose Luis Soriano Roa
        *Fecha de creación: 11/03/2020
        *Descripcion : Metodo que trae el registro que contiene la informacion de descripcion del parque 
        *Este metodo recibe : No resive parametros
        * Retorna: registro que contiene la informacion de la pagina de inicio de la web.
        */

        public UInformacionParque  informacionInicioWeb ()
        {
            using (var db = new Mapeo())
            {
                try { 

                    return db.informacionParque.Where(x => x.Property.Equals("descripcion")).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public UInformacionParque ObtenerInfoById(int id)
        {
            using (var db = new Mapeo())
            {
                try
                {
                    return db.informacionParque
                             .Where(x => x.Id == id)
                             .FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /**
   * Autor: Gabriel Zapata
   * fecha: 28/04/2020
   */
        public bool ActualizarTerminosYCond(UInformacionParque infoParque)
        {

            using (var db = new Mapeo())
            {
                try
                {
                    db.Entry(infoParque).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Existe(infoParque.Id))
                    {
                        return false;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }


        public bool Existe(int id)
        {
            using (var db = new Mapeo())
            {
                return db.informacionParque.Any(x => x.Id == id);
            }
        }



    }
}
