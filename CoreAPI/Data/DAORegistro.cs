using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;


namespace Data
{
    public class DAORegistro
    {
        public URegistroUser Validacion_ExistenciaCorreoYCC(string json_ValidaEmailYCC)
        {
            using (var db = new Mapeo())
            {

                URegistroUser datos = JsonConvert.DeserializeObject<URegistroUser>(json_ValidaEmailYCC);
                URegistroUser validado = new URegistroUser();

                if (db.Usuarios.Where(x => x.CorreoElectronico == datos.CorreoElectronico).FirstOrDefault() != null)
                {
                    validado.EmailExistente = true;
                }
                else
                {
                    validado.EmailExistente = false;
                }
                if (db.Usuarios.Where(x => x.NumeroDocumento == datos.NumDocumento).FirstOrDefault() != null)
                {
                    validado.CedulaExistente = true;
                }
                else
                {
                    validado.CedulaExistente = false;
                }
                return validado;
            }
        }


    }
}
