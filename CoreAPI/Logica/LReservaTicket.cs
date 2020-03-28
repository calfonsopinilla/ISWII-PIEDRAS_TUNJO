using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Logica
{
    public class LReservaTicket
    {
        public IEnumerable<UReservaTicket> ObtenerTickets()
        {
            return new DaoReservaTicket().ObtenerReservas();
        }

        public UReservaTicket Buscar(int id)
        {
            return new DaoReservaTicket().Buscar(id);
        }

        public IEnumerable<UReservaTicket> ObtenerPorUser(int user_id)
        {
            return new DaoReservaTicket().ObtenerPorUser(user_id);
        }

        public bool NuevaReserva(UReservaTicket reserva)
        {
            return new DaoReservaTicket().NuevaReserva(reserva);
        }

        public bool ActualizarReserva(int id, UReservaTicket reserva)
        {
            return new DaoReservaTicket().ActualizarReserva(id, reserva);
        }

        public bool EliminarReserva(int id)
        {
            return new DaoReservaTicket().EliminarReserva(id);
        }
    }
}
