using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data {

    public class DAOCuenta {
        
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

        /*
         Steven Cruz
         Fecha: 18/03/2020
             */
        public UUsuario IniciarSesion(string correo, string clave)
        {
            using (var db = new Mapeo())
            {
                return db.Usuarios.Where(x => x.CorreoElectronico == correo && x.Clave == clave).FirstOrDefault();
            }
        }


        /*
         * Autor: Steven Cruz
         * Fecha: 31/03/2020
         * Desc: Validar si el correo del registro ya existe
         */
        public bool ExisteCorreo(string correo)
        {
            using(var db = new Mapeo())
            {
                return db.Usuarios.Where(x => x.CorreoElectronico == correo).FirstOrDefault() != null;
            }
        }

        /*
         * Autor: Steven Cruz
         * Fecha: 31/03/2020
         * Desc: Validar si el número de documento del registro ya existe
         */
        public bool ExisteNumeroDoc(string numeroDoc)
        {
            using (var db = new Mapeo())
            {
                return db.Usuarios.Where(x => x.NumeroDocumento == numeroDoc).FirstOrDefault() != null;
            }
        }
    }
}
