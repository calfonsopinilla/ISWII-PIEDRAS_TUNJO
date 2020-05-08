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

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 07/05/2020
            Descripcion: Método que sirve para buscar usuarios filtrados por correo electrónico y numero de documento
            Parámetros: String correoElectronico, String numeroDocumento
            Retorna: Usuario filtrado por correo electrónico y numero de documento
        */
        public UUsuario LeerUsuario(string correoElectronico, string numeroDocumento) {

            try {

                using (var db = new Mapeo()) {

                    return db.Usuarios.Where(x =>
                        x.CorreoElectronico.Equals(correoElectronico) &&
                        x.NumeroDocumento.Equals(numeroDocumento) &&
                        x.VerificacionCuenta == true).FirstOrDefault();
                }

            } catch (Exception ex) { throw ex; }
        }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 07/05/2020
            Descripcion: Método que sirve para guardar el token en la base de datos
            Parámetros: URecuperarClave recuperarClave - Objeto con los datos a guardar
            Retorna: True si la inserción se hace de manera correcto, False si ocurre un error
        */
        public bool CrearCodigoRecuperacion(URecuperarClave recuperarClave) {

            try {

                using (var db = new Mapeo()) {

                    db.RecuperarClave.Add(recuperarClave);
                    db.SaveChanges();
                    return true;
                }

            } catch { return false; }
        }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 07/05/2020
            Descripcion: Método que sirve para leer el token en la base de datos para recuperar contraseña
            Parámetros: string token - Código de verificación
            Retorna: Objeto con los datos del recuperar clave
        */
        public URecuperarClave LeerRecuperarUserId(int userId) {

            try {

                using (var db = new Mapeo()) {

                    return db.RecuperarClave.Where(x =>
                        x.UserId == userId).FirstOrDefault();
                }

            } catch (Exception ex) { throw ex; }
        }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 07/05/2020
            Descripcion: Método que sirve para leer el token en la base de datos para recuperar contraseña
            Parámetros: string token - Código de verificación
            Retorna: Objeto con los datos del recuperar clave
        */
        public URecuperarClave LeerRecuperarClave(string token) {

            using (var db = new Mapeo()) {

                return db.RecuperarClave.Where(x => 
                    x.Token.Equals(token)
                    ).FirstOrDefault();
            }
        }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 07/05/2020
            Descripcion: Método que sirve para borrar el token en la base de datos para recuperar contraseña
            Parámetros: URecuperarClave recuperarClave - Objeto con los datos a guardar
            Retorna: True si el borrado fue correcto
        */
        public bool BorrarRecuperarClave(URecuperarClave recuperarClave) {

            using (var db = new Mapeo()) {

                try {

                    db.RecuperarClave.Attach(recuperarClave);
                    db.Entry(recuperarClave).State = EntityState.Deleted;
                    db.SaveChanges();
                    return true;

                } catch(Exception) { return false; }
            }
        }
    }
}
