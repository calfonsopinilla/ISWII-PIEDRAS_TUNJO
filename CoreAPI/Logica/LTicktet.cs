using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using Data;
namespace Logica
{
    public class LTicktet
    {


        public IEnumerable<UTicket> ObtenerTicktes()
        {
            return new DaoTicket().listaTicket();

        }
        public UTicket BuscarTicket(int id)
        {
            return new DaoTicket().BuscarTicket(id);
        }
        public bool agregarTicktet(UTicket ticket)
        {

            return new DaoTicket().AgregarTicktet(ticket);

        }
        public bool actualizaticktet(int id, UTicket ticket)
        {

            return new DaoTicket().actualizarTicket(id, ticket);

        }

        public bool eliminarTicket(int id)
        {
            return new DaoTicket().eliminarTickets(id);
        }

    }
}
