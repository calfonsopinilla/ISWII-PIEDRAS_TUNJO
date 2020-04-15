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
    public class DaoReservaCabana
    {
        private readonly Mapeo db = new Mapeo();

        public IEnumerable<UReservaCabana> ObtenerTodos()
        {
            var reservas = db.ReservaCabanas
                              .Include(x => x.UCabana)
                             .Where(x => x.FechaReserva >= DateTime.Today).ToList();
            return reservas;
        }

        public IEnumerable<UReservaCabana> ObtenerPorUsuario(int userId)
        {
            var reservas = db.ReservaCabanas
                            .Include("UCabana")
                            .Where(x => x.UUsuarioId == userId && x.FechaReserva >= DateTime.Today)
                            .OrderBy(x => x.FechaReserva)
                            .ToList();
            return reservas;
        }

        public UReservaCabana Buscar(int id)
        {
            return db.ReservaCabanas.Find(id);
        }

        public bool Agregar(UReservaCabana reserva)
        {
            var created = db.ReservaCabanas.Add(reserva);
            db.SaveChanges();
            return created != null;
        }

        public bool Actualizar(UReservaCabana reserva, int id)
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

        public bool Eliminar(int id)
        {
            try
            {
                var reserva = db.ReservaCabanas.Find(id);
                db.ReservaCabanas.Remove(reserva);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Existe(int id)
        {
            return db.ReservaCabanas.Any(x => x.Id == id);
        }
    }
}
