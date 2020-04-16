using Data;
using Logic;
using Newtonsoft.Json;
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

        /*
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

        */
        public double CalcularPrecio(int userId)
        {
            try
            {
                var user = new LUsuario().Buscar(userId);

                if (user.VerificacionCuenta == true)
                {

                    if (user.LugarExpedicion.ToLower().Equals("facatativa") || user.LugarExpedicion.ToLower().Equals("facatativá"))
                    {
                        var ticket = new LTicktet().BuscarTicket(3);
                        return ticket.Precio;
                    }
                    else
                    {
                        //permitir comprar el ticket normal
                        var ticket = new LTicktet().BuscarTicket(4);
                        return ticket.Precio;
                    }
                }
                else
                {
                    var ticket = new LTicktet().BuscarTicket(3);
                    return ticket.Precio;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool validarResidencia(int idUser)
        {

            try
            {
                var user = new LUsuario().Buscar(idUser);
                if (user.VerificacionCuenta == true)
                {
                    if (user.LugarExpedicion.ToLower().Equals("facatativa") || user.LugarExpedicion.ToLower().Equals("facatativá"))
                    {
                        bool validarEdad = validarEdades(idUser);
                        if (validarEdad == true)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool validarEdades(int idUser)
        {

            var user = new LUsuario().Buscar(idUser);
            // Menores de 5 años y mayores de 65 años - Exentos de pago
            if (user.VerificacionCuenta == true)
            {
                int edad = CalcularEdad(user.FechaNacimiento);
                if (edad < 5 || edad > 65)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public int CalcularEdad(DateTime fechaNac)
        {
            return DateTime.Today.AddTicks(-fechaNac.Ticks).Year - 1;
        }

        /*
         * Autor: Jhonattan Pulido
         * Descripcion: Método que funciona para buscar una reserva filtrado por ticket
         * Parametros: String token - Valor del token para filtrar
         * Retorna: Objeto tipo reserva token
         */
        public UReservaTicket LeerToken(string token) {

            return new DaoReservaTicket().LeerToken(token);
        }
    }
}
