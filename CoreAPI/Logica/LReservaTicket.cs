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
        /*
            * Autor: Jhonattan Pulido
            * Descripcion: Método que funciona para buscar las reservas de un usuario filtrado por el id
            * Parámetros: String numeroDocumento: numero de documento del usuario
            * Retorna: La reserva buscada
        */
        public UReservaTicket LeerReservaDNI(string numeroDocumento) { return new DaoReservaTicket().LeerReservaDNI(numeroDocumento); }

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


        public List<DateTime> fechasValidas()
        {

            List<DateTime> diasHabliles = diasHabilesTicket();

            if (DateTime.Now.Hour >= 17)
            {
                diasHabliles.RemoveAt(0);
            }

            return diasHabliles.OrderBy(x => x.Month & x.Year).ToList();

        }

        /*
       * Autor: Jose Luis Soriano Roa 
       * Fecha de modificación: 21-04-2020
       * id del usuario
       * Envia  dinamicamente los dias que puede comprar ticket segun los dias de abierto al publico 
       * Como tambien los dias que no pueda reservar cierto tipo de tickets.
       */


        public IEnumerable<DateTime> ObtenerDiasHabilesTicketUser(int idUser, int idTicket){            

            List<DateTime> diasHabiles = diasHabilesTicket();
            List<UReservaTicket> reservasTicket = new DaoReservaTicket().ObtenerReservas().Where(x => x.UUsuarioId == idUser && x.idTicket == idTicket).ToList();

            if (reservasTicket.Count != 0){

                for (int x = 0; x < reservasTicket.Count(); x++){

                    bool buscar = diasHabiles.Contains(reservasTicket[x].FechaIngreso);
                    if (buscar != false){
                        int id = diasHabiles.IndexOf(reservasTicket[x].FechaIngreso);
                        diasHabiles.RemoveAt(id);
                    }
                }
                return diasHabiles;


            }
            else {
                return diasHabiles;
                    
            }

            
        }







        public List<DateTime> diasHabilesTicket(){

                List<DateTime> diashabiles = new List<DateTime>();

                List<DateTime> diasFestivos = new LReservaCabana().festivos(DateTime.Now, DateTime.Now.AddMonths(2)).OrderBy(x => x.Month).ToList();
                DateTime diaAnterior;
                DateTime diaBusqueda;
                for (DateTime i = DateTime.Now; i <= DateTime.Now.AddMonths(1);)
                {
                    if ((int)i.DayOfWeek != 1 && ((int)i.DayOfWeek != 2))
                    {
                        diashabiles.Add(new DateTime(i.Year, i.Month, i.Day));
                    }
                    else if ((int)i.DayOfWeek == 1)
                    {
                        try
                        {
                            DateTime festivo = diasFestivos.Where(x => x.Equals(i.Date)).First();
                            diashabiles.Add(new DateTime(i.Year, i.Month, i.Day));
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    else if ((int)i.DayOfWeek == 2)
                    {
                        diaAnterior = i.AddDays(-1).Date;

                        diaBusqueda = i;
                        try
                        {
                            DateTime festivo = diasFestivos.Where(x => x.Equals(diaAnterior)).First();
                        }
                        catch (Exception ex)
                        {
                            diashabiles.Add(new DateTime(i.Year, i.Month, i.Day));
                        }

                    }

                    i = i.AddDays(1);
                }

            return diashabiles;

        }

        public string validarTipos(string tipo, int idUser, DateTime fecha)
        {

            //si ese usuario ya tiene una reserva para ese dia de tipo 1

            if (tipo.Equals("1"))
            {

                int cantidad = ObtenerTickets().Where(x => x.UUsuarioId == idUser && x.idTicket == 3 && x.FechaIngreso.Date == fecha.Date).Count();

                if (cantidad > 0)
                {
                    return "3";
                }
                else
                {
                    return "1";
                }
            }
            else if (tipo.Equals("3"))
            {
                int cantidad = ObtenerTickets().Where(x => x.UUsuarioId == idUser && x.idTicket == 5 && x.FechaIngreso.Date == fecha.Date).Count();
                if (cantidad > 0)
                {
                    return "2";
                }
                else
                {
                    return "1";
                }
            }
            else if (tipo.Equals("2"))
            {
                return "1";
            }
            else
            {

                return "bad resquest";
            }



        }




    }




}

