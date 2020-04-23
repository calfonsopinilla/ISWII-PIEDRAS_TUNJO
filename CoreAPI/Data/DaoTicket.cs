using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
namespace Data
{
    public class DaoTicket
    {
        private readonly Mapeo db = new Mapeo();

        public IEnumerable<UTicket> listaTicket() {
            using (var db = new Mapeo()) {
                try {
                    return db.Tickets.ToList();
                } catch (Exception ex) { throw ex; }
            }
        }

        /**
         * @Autor Gabriel Andres Zapata Morera
         * Fecha: 14/04/2020
         * Metodo que obtiene listado de todos los tipos tickets en base de datos
         *  
         */
        public List<UTicket> ObtenerTickets()
        {
            using (var db = new Mapeo())
            {
                try
                {
                    return db.Tickets.ToList();
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
         * Parametro de recepcion: string subscripcion a buscar y double valor a buscar
         * return string busqueda
         **/

        public string Valida_ExistenciaTicket(string nombreT, double precio)
        {
            string busqueda = "";
            using (var db = new Mapeo())
            {
                if (db.Tickets.Where(x => x.Nombre == nombreT && x.Precio == precio).FirstOrDefault() != null)
                {
                    /*
                        if (db.Tickets.Where(x => x.Nombre == nombreT).FirstOrDefault() != null && 
                        (db.Tickets.Where(x => x.Precio == precio).FirstOrDefault() != null))
                    */
                    busqueda = "El ticket ya ha sido creado con ese precio";
                }
                //((db.Tickets.Where(x => x.Nombre == nombreT).FirstOrDefault() != null) &&
                else if (db.Tickets.Where(x => x.Nombre == nombreT && x.Precio != precio).FirstOrDefault() != null){                
                    busqueda = "El ticket ya ha sido creada con diferente precio";
                }
                else
                {
                    busqueda = "El ticket no ha sido creado";
                }

            }

            return busqueda;
        }



        public bool AgregarTicktet(UTicket ticket)
        {
            using (var db = new Mapeo())
            {
                try
                {
                    db.Tickets.Add(ticket);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public bool actualizarTicket(int id, UTicket ticket)
        {

            using (var db = new Mapeo())
            {
                try
                {
                    db.Entry(ticket).State = EntityState.Modified;
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
        }


        public bool Existe(int id)
        {
            using (var db = new Mapeo())
            {
                return db.Tickets.Any(x => x.Id == id);
            }
        }

        public bool eliminarTickets(int id)
        {
            using (var db = new Mapeo())
            {
                try
                {
                    var ticktes = db.Tickets.Find(id);
                    db.Tickets.Remove(ticktes);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool CambiarEstado(int id)
        {
            using (var db = new Mapeo())
            {
                try
                {
                    var ticktes = db.Tickets.Find(id);
                    ticktes.Estado = 2;
                    db.Entry(ticktes).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public bool HabilitarTickets(int id)
        {
            using (var db = new Mapeo())
            {
                try
                {
                    var ticktes = db.Tickets.Find(id);
                    ticktes.Estado = 1;
                    db.Entry(ticktes).State = EntityState.Modified;                    
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        
        public UTicket BuscarTicket(int id)
        {
            try
            {
                using (var db = new Mapeo()) {
                    return db.Tickets.Find(id);
                }
                    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }



}
