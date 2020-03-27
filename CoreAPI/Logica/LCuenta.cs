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
                return new DAOCuenta().IniciarSesion(correo, clave);
            } catch (Exception ex) {
                throw ex;
            }
        }

    }
        
               
}

