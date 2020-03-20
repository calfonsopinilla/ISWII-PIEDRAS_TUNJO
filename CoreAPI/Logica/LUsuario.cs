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
    public class LUsuario{



        /*
      @Autor : Jose Luis Soriano Roa
      *Fecha de creación: 18/03/2020
      *Descripcion : Metodo que trae la informacion del usuario que va a iniciar sesion.
      *Este metodo recibe : Resive un objeto de tipo Ulogin quien incorpora correoElectronico y clave en json 
      * Retorna: Si el usuario es encontrado retorna toda la informacion de el, si el usuario no ha realizado
      * la verificacion del correo retorna 1 y si el usuario no esta registrado retorna 2
      */

        public string iniciarSesion(string datosLogin) {

            try
            {
                UUsuario usuario = new DAOUsuario().iniciarSesion(JsonConvert.DeserializeObject<ULogin>(datosLogin));

                if (usuario == null) {
                    return "2";
                } else if (usuario != null && usuario.VerificacionCuenta == true) {
                    return JsonConvert.SerializeObject(usuario);
                } else  {
                    return "1";
                }

            } catch (Exception ex) {
                throw ex;
            }
        }

        }
        
               
}

