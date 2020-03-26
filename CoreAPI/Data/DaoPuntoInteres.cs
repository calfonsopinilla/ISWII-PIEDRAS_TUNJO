using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data
{
    public class DaoPuntoInteres
    {
        // Variables
        private readonly Mapeo db = new Mapeo();
        private UPuntoInteres puntoInteres;

        public IEnumerable<UPuntoInteres> ObtenerTodos()
        {
            try
            {
                return db.PuntosInteres.ToList();
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 26/03/2020
            Descripción: Método que sirve para guardar un punto de interes en la tabla puntos interes
            Recibe: UPuntoInteres puntoInteres - Objeto con los atributos a guardar
            Retorna: Booleano
        */
        public bool CrearPuntoInteres(UPuntoInteres puntoInteres) {

            try {

                using (this.db) {

                    this.db.PuntosInteres.Add(puntoInteres);
                    this.db.SaveChanges();
                    return true;
                }

            } catch(Exception ex) {
                
                throw ex;                
            }
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 26/03/2020
            Descripción: Método que sirve para guardar un punto de interes en la tabla puntos interes
            Recibe: Integer id - Id del punto de interes que se desea buscar
            Retorna: Objeto de tipo UPuntoInteres con los datos filtrados
        */
        public UPuntoInteres LeerPuntoInteres(int id) {

            using (this.db) {

                try {

                    this.puntoInteres = this.db.PuntosInteres.Find(id);
                    return this.puntoInteres;

                } catch (Exception ex) { throw ex; }
            }
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 26/03/2020
            Descripción: Método que sirve para guardar un punto de interes en la tabla puntos interes
            Recibe: Integer id - Id del punto de interes que se desea buscar
            Retorna: Booleano definiendo si la elmininacion se hizo satisfactoriamente
        */
        public bool BorrarPuntoInteres(int id) {

            using (this.db) {

                try {

                    this.puntoInteres = this.db.PuntosInteres.Find(id);
                    this.db.PuntosInteres.Remove(this.puntoInteres);
                    return true;

                } catch (Exception ex) { throw ex; }
            }
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 26/03/2020
            Descripción: Método que sirve para guardar un punto de interes en la tabla puntos interes
            Recibe: UPuntoInteres puntoInteres - Objeto con los atributos a actualizar
            Retorna: Booleano
        */
        public bool ActualizarPuntoInteres(UPuntoInteres puntoInteres) {

            using (this.db) {

                try {

                    this.puntoInteres = this.db.PuntosInteres.Where(
                        x => x.Id == puntoInteres.Id).SingleOrDefault();

                    if (this.puntoInteres != null) {

                        this.db.Entry(this.puntoInteres).CurrentValues.SetValues(puntoInteres);
                        this.db.SaveChanges();
                        return true;
                    }

                    return false;

                } catch (Exception ex) { throw ex; }
            }
        }
    }
}
