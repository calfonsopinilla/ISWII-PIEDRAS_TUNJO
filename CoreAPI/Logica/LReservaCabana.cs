using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Logica
{
    public class LReservaCabana
    {
        public IEnumerable<UReservaCabana> ObtenerTodos()
        {
            return new DaoReservaCabana().ObtenerTodos();
        }

        public IEnumerable<UReservaCabana> ObtenerPorUsuario(int userId)
        {
            return new DaoReservaCabana().ObtenerPorUsuario(userId);
        }

        public UReservaCabana Buscar(int id)
        {
            return new DaoReservaCabana().Buscar(id);
        }

        public bool Agregar(UReservaCabana reserva)
        {
            return new DaoReservaCabana().Agregar(reserva);
        }

        public bool Actualizar(UReservaCabana reserva, int id)
        {
            return new DaoReservaCabana().Actualizar(reserva, id);
        }

        public bool Eliminar(int id)
        {
            return new DaoReservaCabana().Eliminar(id);
        }
    }
}
