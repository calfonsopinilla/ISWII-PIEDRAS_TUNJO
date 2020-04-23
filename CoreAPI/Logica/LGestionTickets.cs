using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using Data;
using Newtonsoft.Json;

namespace Logica
{
 
    public class LGestionTickets
    {
        /**
         * Autor: Gabriel Zapata
         * fecha: 14/04/2020
         * Parametro de recepcion: estadoFiltro
         * Return: lista de tickets, segun estado
         **/  
        public List<UTicket> ObtenerTickets_Filtrados(int estadoFiltro)
        {
            try
            {
                List<UTicket> tickets = new DaoTicket().ObtenerTickets();
                List<UTicket> listadoTickets = new List<UTicket>();
                if (tickets.Count < 0)
                {
                    return null;
                }
                else
                {
                    for (int x = 0; x < tickets.Count(); x++)
                    {
                        if (estadoFiltro == 1 && tickets[x].Estado == 1)
                        {
                            listadoTickets.Add(tickets[x]);
                        }
                        else if (estadoFiltro == 2 && tickets[x].Estado == 2)
                        {
                            listadoTickets.Add(tickets[x]);
                        }

                    }
                    //return JsonConvert.SerializeObject(noticias);
                    return listadoTickets;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /**
           * Autor: Gabriel Zapata
           * fecha: 14/04/2020
           * Parametro de recepcion: Objeto tipo UTicket
           * Return: string estado registro de la subscripcion
        **/
        public string RegistroTicket(UTicket datosTicket)
        {
            try
            {
                string estado = new DaoTicket().Valida_ExistenciaTicket(datosTicket.Nombre, datosTicket.Precio);
                int validacion = 0;

                switch (estado)
                {
                    case "El ticket ya ha sido creado con ese precio":
                        validacion = 1; //existe la subscripcion con el mismo valor
                        break;
                    case "El ticket ya ha sido creada con diferente precio":
                        validacion = 2; //existe la subscripcion con diferente valor                
                        break;
                    case "El ticket no ha sido creado":
                        validacion = 3; // no existe la validacion
                        break;

                }
                switch (validacion)
                {
                    case 1:
                        return estado;
                    case 2:
                        return estado;
                    case 3:
                        try
                        {
                            datosTicket.Estado = 1;
                            datosTicket.Token = "";
                            datosTicket.LastModificacion = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
                            new DaoTicket().AgregarTicktet(datosTicket);
                            estado = "Ticket insertado satisfactoriamente";
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        break;

                }
                return estado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /**
         * Autor: Gabriel Zapata
         * fecha: 14/04/2020
         * Parametro de recepcion: id del ticket
         * Return: Objeto UTicket
         **/
        public UTicket Buscar(int id)
        {
            return new DaoTicket().BuscarTicket(id);
        }
        /**
         * Autor: Gabriel Zapata
         * fecha: 14/04/2020
         * Parametro de recepcion: id del ticket, y la info a modificar
         * Return: bool 
         **/
        public bool Actualizar(int id, UTicket ticket)
        {
            ticket.Estado = 1;
            ticket.Token = "";
            ticket.LastModificacion = DateTime.Now;
            return new DaoTicket().actualizarTicket(id, ticket);
        }

        /**
         * Autor: Gabriel Zapata
         * fecha: 14/04/2020
         * Descripcion: Metodo para cambiar de estado el ticket
         * Recibe: id del ticket
         * Retorna: bool de confirmacion
         **/
        public bool EliminarTicket(int id)
        {
            try
            {               
                new DaoTicket().CambiarEstado(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /**
        * Autor: Gabriel Zapata
        * fecha: 20/04/2020
        * Descripcion: Metodo para cambiar de estado el ticket
        * Recibe: id del ticket
        * Retorna: bool de confirmacion
        **/
        public bool HabilitarTicket(int id, int estado)
        {
            try
            {
                if (estado == 1)
                {
                    new DaoTicket().HabilitarTickets(id);
                    return true;
                }
                else if (estado == 2)
                {
                    new DaoTicket().CambiarEstado(id);
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
