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

        public string informacionUsuarios(string datosJson)
        {
            try
            {
                //deserializar lo que llegue 
                UBusqueda busqueda = JsonConvert.DeserializeObject<UBusqueda>(datosJson);

                List<UUsuario> usuarios = new DaoAdministradorUsuario().informacionUsuarios();

                if (!String.IsNullOrEmpty(busqueda.RolId) && !String.IsNullOrEmpty(busqueda.NumeroDocumento))
                {

                    return JsonConvert.SerializeObject(usuarios.Where(x => x.RolId.Equals(busqueda.RolId) && x.NumeroDocumento.Equals(busqueda.NumeroDocumento)).ToList());

                }
                else if (!String.IsNullOrEmpty(busqueda.RolId) && String.IsNullOrEmpty(busqueda.NumeroDocumento))
                {

                    return JsonConvert.SerializeObject(usuarios.Where(x => x.RolId.Equals(busqueda.RolId)).ToList());

                }
                else if (String.IsNullOrEmpty(busqueda.RolId) && !String.IsNullOrEmpty(busqueda.NumeroDocumento))
                {

                    return JsonConvert.SerializeObject(usuarios.Where(x => x.NumeroDocumento.Equals(busqueda.NumeroDocumento)).ToList());
                }
                else
                {
                    return JsonConvert.SerializeObject(usuarios);

                }
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
    }


}
