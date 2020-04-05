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

        public  List<UUsuario> ObtenerUsuarios()
        {
            try
            {
                return new DaoUsuario().ObtenerUsuarios();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*
         * Autor: Steven Cruz
         * Fecha: 31/03/2020
         * Descripción: Agregar usuario validando que el número de documento y correo no se repita
         * Retorna un object { boolean, string }
         */
        public object Agregar(UUsuario usuario)
        {
            string errorMessage = string.Empty;
            bool agregado = false;
            try
            {
                if (BuscarPorNumeroDoc(usuario.NumeroDocumento) != null)
                {
                    errorMessage = "Ya existe una cuenta con ese número de documento";
                } else if (BuscarPorCorreo(usuario.CorreoElectronico) != null)
                {
                    errorMessage = "Ya existe una cuenta con ese correo electrónico";
                } else
                {
                    agregado = new DaoUsuario().Agregar(usuario);
                }
            }catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new { ok = false, message = errorMessage };
            } else
            {
                return new { ok = agregado };
            }
        }

        public UUsuario Buscar(int id)
        {
            return new DaoUsuario().Buscar(id);
        }

        /*
        * Autor: Steven Cruz
        * Fecha: 31/03/2020
        * Descripción: Buscar usuario por cédula
        */
        public UUsuario BuscarPorNumeroDoc(string numeroDocumento)
        {
            var usuarios = ObtenerUsuarios();
            return usuarios.Where(x => x.NumeroDocumento == numeroDocumento).FirstOrDefault();

        }
        /*
         * Autor: Steven Cruz
         * Fecha: 31/03/2020
         * Descripción: Buscar usuario por correo
         */
        public UUsuario BuscarPorCorreo(string correo)
        {
            var usuarios = ObtenerUsuarios();
            return usuarios.Where(x => x.CorreoElectronico == correo).FirstOrDefault();
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
