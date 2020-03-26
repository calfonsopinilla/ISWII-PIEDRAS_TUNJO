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

        public IEnumerable<UTicket> listaTicket() {
            using (var db = new Mapeo()) {
                try {
                    return db.Tickets.ToList();
                } catch (Exception ex) { throw ex; }
            }
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
                return db.pqr.Any(x => x.Id == id);
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
