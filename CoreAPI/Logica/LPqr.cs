using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using Data;
namespace Logica
{
    public class LPqr
    {

        public IEnumerable<UPQR> ObtenerPqr()
        {
            return new DaoPqr().ListaPQR();
        }
        
        public IEnumerable<UPQR> ObtenerPorUser(int user_id)
        {
            return new DaoPqr().ObtenerPorUsuario(user_id);
        }

        public UPQR BuscarPqr(int id)
        {
            return new DaoPqr().BuscarPqr(id);
        }
        public bool agregarPqr(UPQR pqr) {

            return new DaoPqr().AgregarPqr(pqr);

        }
        public bool actualizaPqr(int id,UPQR pqr) {

            return new DaoPqr().actualizarPqr(id, pqr);

        }
        public bool eliminarPqr (int id)
        {
            return new DaoPqr().eliminarPqr(id);
        }

    }
}
