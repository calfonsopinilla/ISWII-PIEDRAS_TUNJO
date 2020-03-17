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
    public class LAdministradorUsuario
    {


        /*
       @Autor : Jose Luis Soriano Roa
       *Fecha de creación: 11/03/2020
       *Descripcion : metodo que envia la informacion de los usuarios al servicio
       *Este metodo recibe : Resive el id de el rol o la cedula correspondiente a filtrar
       * Retorna: lista de la informacion de los usuarios filtrada por cedula y rol en json 
       */

        public  List<UUsuario>    informacionUsuarios()
        {
            try
            {
                //deserializar lo que llegue 

                return new DaoAdministradorUsuario().informacionUsuarios();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public string agregarUsuario(string datosJson) {
            try {
                
                UUsuario usuario = JsonConvert.DeserializeObject<UUsuario>(datosJson);
                if ((new DaoAdministradorUsuario().buscarCedula(usuario.NumeroDocumento)) == true && new DaoAdministradorUsuario().buscarCorreo(usuario.CorreoElectronico) == true)
                {
                    //agregar
                    DaoAdministradorUsuario daoAdministradorUsuario = new DaoAdministradorUsuario();
                    daoAdministradorUsuario.agregarUsuario(usuario);
                    return "1";
                }
                else if ((new DaoAdministradorUsuario().buscarCedula(usuario.NumeroDocumento)) == true && new DaoAdministradorUsuario().buscarCorreo(usuario.CorreoElectronico) == false)
                {
                    return "2"; //el correo ya esta registrado 
                }
                else if ((new DaoAdministradorUsuario().buscarCedula(usuario.NumeroDocumento)) == false && new DaoAdministradorUsuario().buscarCorreo(usuario.CorreoElectronico) == false)
                {
                    // 
                    return "3"; // la cedula ya esta registrada
                }
                else {

                    return "4"; // la cedula  y el correo ya estan registrados

                }
            } catch (Exception ex) {
                throw ex;
            }
        }
        /*
       @Autor : Carlos Alfonso Pinilla Garzón
       *Fecha de creación: 13/03/2020
       *Descripcion: Metodo para cambia el estado de la cuenta del usuario
       *Este metodo recibe: 
       *Retorna:
       */
       public bool cambiarEstado(string cedula)
        {
            try
            {
                DaoAdministradorUsuario user = new DaoAdministradorUsuario();
                //if (user.buscarCedula(double.Parse(cedula)) != false)
                if (user.buscarCedula(cedula) != false)
                {
                    user.cambiarEstado(double.Parse(cedula));
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /*
       @Autor : Carlos Alfonso Pinilla Garzón
       *Fecha de creación: 13/03/2020
       *Descripcion: Metodo para actualizar los datos del usuario
       *Este metodo recibe: 
       *Retorna:
       */
       public string actualizarUsuario(string datosJson)
        {
            try
            {
                UUsuario usuario = JsonConvert.DeserializeObject<UUsuario>(datosJson);
                if ((new DaoAdministradorUsuario().buscarCedula(usuario.NumeroDocumento)) == true && new DaoAdministradorUsuario().buscarCorreo(usuario.CorreoElectronico) == true)
                {
                    DaoAdministradorUsuario daoAdministradorUsuario = new DaoAdministradorUsuario();
                    daoAdministradorUsuario.actualizarUsuario(usuario);
                    return "1";//Lo datos se actualizan
                }
                else if ((new DaoAdministradorUsuario().buscarCedula(usuario.NumeroDocumento)) == true && new DaoAdministradorUsuario().buscarCorreo(usuario.CorreoElectronico) == false)
                {
                    return "2";//EL correo ya esta registrado
                }
                else if ((new DaoAdministradorUsuario().buscarCedula(usuario.NumeroDocumento)) == false && new DaoAdministradorUsuario().buscarCorreo(usuario.CorreoElectronico) == false)
                {
                    return "3";//La cedula ya esta registrada
                }
                else
                {
                    return "4";//La cedula y el correo ya estan registrado
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
