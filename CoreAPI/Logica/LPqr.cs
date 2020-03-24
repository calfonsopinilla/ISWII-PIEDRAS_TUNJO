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

        public IEnumerable<UpqrInformacion> ObtenerPqr()
        {
            return new DaoPqr().listaPqr();
        }

        public UpqrInformacion BuscarPqr(int id)
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
