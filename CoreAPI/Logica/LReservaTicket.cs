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

        public int CalcularPrecio(int userId)
        {
            try
            {
                var user = new LUsuario().Buscar(userId);

                // Nacidos en Facatativá - $1800
                if (user.LugarExpedicion.ToLower().Equals("facatativa") || user.LugarExpedicion.ToLower().Equals("facatativá"))
                {
                    return 1800;
                }

                // Niños de los 5 a los 10 años - $1800
                int edad = CalcularEdad(user.FechaNacimiento);
                if (edad >= 5 && edad <= 10)
                {
                    return 1800;
                }

                // Menores de 5 años y mayores de 65 años - Exentos de pago
                if (edad < 5 && edad > 65)
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Si no cumple ninguna de las condiciones, paga 4800
            return 4800;
        }

        public int CalcularEdad(DateTime fechaNac)
        {
            return DateTime.Today.AddTicks(-fechaNac.Ticks).Year - 1;
        }
    }
}
