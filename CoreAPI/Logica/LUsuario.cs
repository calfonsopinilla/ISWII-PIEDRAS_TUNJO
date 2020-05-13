using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using Data;
using Newtonsoft.Json;

namespace Logica {

    public class LUsuario {

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Servicio que funciona para traer los estados que estan registrados pero no han sido verificados por el administrador
            * Parámetros: Ninguno
            * Retorna: Lista de usuarios filtrados por verificación de cuenta en falso
        */
        public List<UUsuario> LeerUsuariosNoVerificados() { return new DaoUsuario().LeerUsuariosNoVerificados(); }

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
        /**
         * @Autor : Gabriel Andres Zapata Morera
         * Fecha de creación: 14/04/2020
         * Descripcion : Metodo que devuelve la informacion de los usuarios al servicio
         * dependiendo del valor del filtro
         */
        public List<UUsuario> ObtenerUsuarios_Filtrados(int estadoFiltro)               
        {
            try
            {
                List<UUsuario> usuarios = new DaoUsuario().ObtenerUsuarios();
                List<UUsuario> listadoUsuarios = new List<UUsuario>();
                if (usuarios.Count < 0)
                {
                    return null;
                }
                else
                {
                    for (int x = 0; x < usuarios.Count(); x++)
                    {
                        if (estadoFiltro == 1 && usuarios[x].EstadoCuenta == true)
                        {
                            listadoUsuarios.Add(usuarios[x]);
                        }
                        else if(estadoFiltro == 2 && usuarios[x].EstadoCuenta == false)
                        {
                            listadoUsuarios.Add(usuarios[x]);
                        }

                    }
                    //return JsonConvert.SerializeObject(noticias);
                    return listadoUsuarios;
                }
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

        public bool Actualizar(int id, UUsuario usuario)
        {
            bool validar = new DaoUsuario().Actualizar(id, usuario);
            if (validar == true) {
                return true;
            }
            else {

                return false;
            }
               

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

        /*
          @Autor : Gabriel Andres Zapata Morera
          *Fecha de creación: 14/04/2020
          *Descripcion: Metodo para cambia el estado de la cuenta del usuario
          *Este metodo recibe: estado_filtro y id_usuario
          *Retorna: string validacion
      */
        public string CambiarEstado_Usuarios(int estadoFiltro, int id_Usuario)
        {
            try
            {
                string validacion = "";                
                validacion = new DaoUsuario().CambiarEstado_Usuarios(estadoFiltro, id_Usuario);
                return validacion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /**
        *@Autor : Gabriel Andres Zapata Morera
        *Fecha de creación: 14/04/2020
        *Descripcion: Metodo para cambiar estado de la cuenta del ususario segun el estadoFiltro
        *Este metodo recibe:  estadoFiltro,  id_Usuario
        *Retorna: string validacion
        */
        public List<URol> ObteneRoles()
        {
            return new DaoUsuario().ObtenerRoles();
        }
    }
}
