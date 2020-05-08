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
    public class LCuenta{



        /*
      @Autor : Jose Luis Soriano Roa
      *Fecha de creación: 18/03/2020
      *Descripcion : Metodo que trae la informacion del usuario que va a iniciar sesion.
      *Este metodo recibe : Resive un objeto de tipo Ulogin quien incorpora correoElectronico y clave en json 
      * Retorna: Si el usuario es encontrado retorna toda la informacion de el, si el usuario no ha realizado
      * la verificacion del correo retorna 1 y si el usuario no esta registrado retorna 2
      */

        public UUsuario IniciarSesion(string correo, string clave) {

            try
            {
                UUsuario user = new DAOCuenta().IniciarSesion(correo, clave);
                if (user != null)
                {
                    UToken token = new UToken();
                    string datos = JsonConvert.SerializeObject(user);
                    token.Token = new Encriptar().encriptar(datos);
                    token.UserId = user.Id;
                    token.FechaGeneracion = DateTime.Now;
                    token.FechaVencimiento = DateTime.Now.AddDays(5); DateTime.Now.AddMinutes(30);
                    if (user.RolId == 1 || user.RolId == 3)
                    {
                        token.AplicacionId = 1;
                    }
                    else if (user.RolId == 2 || user.RolId == 4)
                    {
                        token.AplicacionId = 2;
                    }


                    new DaoToken().insertarToken(token);
                }


                return new DAOCuenta().IniciarSesion(correo, clave);
            } catch (Exception ex) {
                throw ex;
            }
        }

        /*
         * Autor: Steven Cruz
         * Fecha: 31/03/2020
         * Desc: Registro de un usuario
         */

        public bool Registrar(UUsuario usuario)
        {
            return new DAOCuenta().Registrar(usuario);
        }

        /*
         * Autor: Steven Cruz
         * Fecha: 31/03/2020
         * Desc: Validar si el correo del registro ya existe
         */
        public bool ExisteCorreo(string correo)
        {
            return new DAOCuenta().ExisteCorreo(correo);
        }

        /*
         * Autor: Steven Cruz
         * Fecha: 31/03/2020
         * Desc: Validar si el número del documento del registro ya existe
         */
        public bool ExisteNumeroDoc(string numeroDoc)
        {
            return new DAOCuenta().ExisteNumeroDoc(numeroDoc);
        }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 07/05/2020
            Descripcion: Método que sirve para buscar usuarios filtrados por correo electrónico y numero de documento
            Parámetros: String correoElectronico, String numeroDocumento
            Retorna: Usuario filtrado por correo electrónico y numero de documento
        */
        public UUsuario LeerUsuario(string correoElectronico, string numeroDocumento) { return new DAOCuenta().LeerUsuario(correoElectronico, numeroDocumento); }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 07/05/2020
            Descripcion: Método que sirve para guardar el token en la base de datos
            Parámetros: URecuperarClave recuperarClave - Objeto con los datos a guardar
            Retorna: True si la inserción se hace de manera correcto, False si ocurre un error
        */
        public bool CrearCodigoRecuperacion(URecuperarClave recuperarClave) { return new DAOCuenta().CrearCodigoRecuperacion(recuperarClave); }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 07/05/2020
            Descripcion: Método que sirve para leer el token en la base de datos para recuperar contraseña
            Parámetros: string token - Código de verificación
            Retorna: Objeto con los datos del recuperar clave
        */
        public URecuperarClave LeerRecuperarClave(string token) { return new DAOCuenta().LeerRecuperarClave(token); }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 07/05/2020
            Descripcion: Método que sirve para borrar el token en la base de datos para recuperar contraseña
            Parámetros: URecuperarClave recuperarClave - Objeto con los datos a guardar
            Retorna: True si el borrado fue correcto
        */
        public bool BorrarRecuperarClave(URecuperarClave recuperarClave) { return new DAOCuenta().BorrarRecuperarClave(recuperarClave); }

        /*
            @Autor: Jhonattan Pulido
            Fecha creación: 07/05/2020
            Descripcion: Método que sirve para leer el token en la base de datos para recuperar contraseña
            Parámetros: string token - Código de verificación
            Retorna: Objeto con los datos del recuperar clave
        */
        public URecuperarClave LeerRecuperarUserId(int userId) { return new DAOCuenta().LeerRecuperarUserId(userId); }
    }
}

