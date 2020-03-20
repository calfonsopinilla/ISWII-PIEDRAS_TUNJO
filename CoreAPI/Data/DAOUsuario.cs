using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data {

    public class DAOUsuario {

        private Mapeo conexionBD;

        /*
            Autor: Jhonattan Pulido
            Fecha modificacion: 8/03/2020
        */
        public bool CrearUsuario(UUsuario usuario) {

            using (this.conexionBD = new Mapeo()) {

                //this.conexionBD.Usuarios.Add(usuario);
                //this.conexionBD.SaveChanges();
            }

            return true;
        }
        
        /*
         Autor: Steven Cruz
         Fecha Modificación: 18/03/2020
         */
        public bool Registrar(UUsuario usuario)
        {
            try
            {
                using (var db = new Mapeo())
                {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    return true;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /**/
        public UUsuario IniciarSesion(string correo, string clave)
        {
            using (var db = new Mapeo())
            {
                return db.Usuarios.Where(x => x.CorreoElectronico == correo && x.Clave == clave).FirstOrDefault();
            }
        }
    }
}
