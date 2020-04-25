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
    public class DaoReservaTicket
    {
        private readonly Mapeo db = new Mapeo();

        public IEnumerable<UReservaTicket> ObtenerReservas()
        {
            try
            {
                return db.ReservaTickets
                         .Include("UUsuario")
                         .ToList();
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<UReservaTicket> ObtenerPorUser(int user_id)
        {
            try
            {
                return db.ReservaTickets
                          .Include("UUsuario")
                          .Where(x => x.UUsuarioId == user_id)
                          .OrderBy(x => x.FechaIngreso)
                          .ToList();
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public UReservaTicket Buscar(int id)
        {
            try
            {
                return db.ReservaTickets
                        .Include("UUsuario")
                        .Where(x => x.Id == id)
                        .FirstOrDefault();
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool NuevaReserva(UReservaTicket reserva)
        {
            try
            {
                db.ReservaTickets.Add(reserva);
                db.SaveChanges();
                return true;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool ActualizarReserva(int id, UReservaTicket reserva)
        {
            try
            {
                db.Entry(reserva).State = EntityState.Modified;
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

        public bool EliminarReserva(int id)
        {
            try
            {
                var reserva = db.ReservaTickets.Find(id);
                db.ReservaTickets.Remove(reserva);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool Existe(int id)
        {
            return db.ReservaTickets.Any(x => x.Id == id);
        }

        /*
         * Autor: Jhonattan Pulido
         * Descripcion: Método que funciona para buscar una reserva filtrado por ticket
         * Parametros: String qr - Valor del qr para filtrar
         * Retorna: Objeto tipo reserva token
         */
        public UReservaTicket LeerToken(string qr) {
            try {

                return db.ReservaTickets
                        .Include("UUsuario")
                        .Where(x => x.Qr == qr && x.EstadoId == 1)
                        .FirstOrDefault();
            } catch (Exception ex) {
                throw ex;
            }
        }

    }
}
