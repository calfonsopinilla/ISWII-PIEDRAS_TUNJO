using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using Data;

namespace Logica
{
    public class LRegistro
    {
        public URegistroUser Validacion_ExistenciaCorreoYCC(string json_EmailYCC)
        {
            return new DAORegistro().Validacion_ExistenciaCorreoYCC(json_EmailYCC);
        }




    }
}
