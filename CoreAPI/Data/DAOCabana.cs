using System;
using System.Collections.Generic;
using System.Linq;
using Utilitarios;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
namespace Data {

    /*
        Autor: Jhonattan Alejandro Pulido Arenas
        Fecha creación: 18/03/2020
        Descripción: Clase que sirve para hacer CRUD de cabañas
    */
    public class DAOCabana {

        // Variables
        private Mapeo conexionBD;
        private UCabana cabana;
        private List<UCabana> listaCabanas;
        private readonly Mapeo db = new Mapeo();
        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para añadir una nueva cabaña a la base de datos
            Recibe: UCabana cabana - Objeto de tipo cabaña con las especificaciones ya validadas
            Retorna: Booleano true
        */
        public bool CrearCabana(UCabana cabana) {

            try {

                using (this.conexionBD = new Mapeo()) {

                    this.conexionBD.Cabana.Add(cabana);
                    this.conexionBD.SaveChanges();
                    return true;
                }

            } catch (Exception ex) {

                throw ex;
            }            
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer una cabaña filtrada por el Id
            Recibe: Integer cabanaId - ID de la cabaña que se desea traer datos
            Retorna: Objeto de tipo cabaña
        */
        public UCabana LeerCabana(int cabanaId) {

            try {

                this.cabana = new UCabana();

                using (this.conexionBD = new Mapeo()) {

                    this.cabana = this.conexionBD.Cabana.Find(cabanaId);                                        
                    return this.cabana;                    
                }                

            } catch (Exception ex) {

                throw ex;
            }
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer una cabaña filtrada por el nombre
            Recibe: String cabanaNombre - Nombre de la cabaña que se desea traer datos
            Retorna: Objeto de tipo cabaña
        */
        public UCabana LeerCabanaNombre(string cabanaNombre) {

            try {

                this.cabana = new UCabana();

                using (this.conexionBD = new Mapeo()) {

                    this.cabana = this.conexionBD.Cabana.Where(x => x.Nombre.Equals(cabanaNombre)).FirstOrDefault();                  
                    return this.cabana;                    
                }                

            } catch (Exception ex) {

                throw ex;
            }
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para leer las cabañas existentes alojadas en la base de datos
            Recibe: Nada
            Retorna: Lista de cabañas
        */
        public IEnumerable<UCabana> LeerCabanas() {

            this.listaCabanas = null;

            try {

                using (this.conexionBD = new Mapeo()) {
                    this.listaCabanas = this.conexionBD.Cabana.OrderBy(x => x.Id).ToList();
                }                

            } catch (Exception ex) {

                throw ex;
            }

            return this.listaCabanas;            
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para borrar una cabaña de la base de datos
            Recibe: Integer cabanaId - El id de la cabaña que se desea eliminar
            Retorna: Booleano true
        */
        public bool BorrarCabana(int cabanaId) {

            try {

                this.cabana = new UCabana();

                using (this.conexionBD = new Mapeo()) {

                    this.cabana = this.conexionBD.Cabana.Find(cabanaId);
                    this.conexionBD.Cabana.Remove(this.cabana);
                    this.conexionBD.SaveChanges();
                    return true;                    
                }                

            } catch (Exception ex) {

                throw ex;
            }
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 18/03/2020
            Descripción: Método que sirve para actuaizar una cabaña de la base de datos
            Recibe: UCabana cabana - Objeto que contiene los datos que se quieren modificar
            Retorna: Booleano true
        */
        public bool ActualizarCabana(UCabana cabana) {

            try {

                this.cabana = new UCabana();

                using (this.conexionBD = new Mapeo()) {

                    this.cabana = this.conexionBD.Cabana.Where(
                        x => x.Id == cabana.Id).SingleOrDefault();

                    if (this.cabana != null) {

                        this.conexionBD.Entry(this.cabana).CurrentValues.SetValues(cabana);
                        this.conexionBD.SaveChanges();
                    }

                    return true;
                }

            } catch (Exception ex) {

                throw ex;
            }               
        }

        public bool Actualizar(int id, UCabana cabana)
        {
            try
            {
                db.Entry(cabana).State = EntityState.Modified;
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
            return db.Cabana.Any(x => x.Id == id);
        }
    }
}
