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
    public class LUsuario
    {


        /*
       @Autor : Jose Luis Soriano Roa
       *Fecha de creación: 11/03/2020
       *Descripcion : metodo que envia la informacion de los usuarios al servicio
       *Este metodo recibe : Resive el id de el rol o la cedula correspondiente a filtrar
       * Retorna: lista de la informacion de los usuarios filtrada por cedula y rol en json 
       */

        public  List<UUsuario> informacionUsuarios()
        {
            try
            {
                return new DaoUsuario().informacionUsuarios();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool agregarUsuario(UUsuario Usuarios)
        {
            return new DaoUsuario().agregarUsuario(Usuarios);
        }

        public UUsuario Buscar(int id)
        {
            return new DaoUsuario().Buscar(id);
        }
        public bool Actualizar(int id, UUsuario usuarios)
        {
            return new DaoUsuario().Actualizar(id, usuarios);
        }

        public bool Eliminar(int id)
        {
            return new DaoUsuario().Eliminar(id);
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
                bool validar = new DaoUsuario().buscarCedula(cedula);
                if (validar == true)
                {
                    return false;
                }
                else
                {
                    DaoUsuario dao = new DaoUsuario();
                    dao.cambiarEstado(cedula);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
