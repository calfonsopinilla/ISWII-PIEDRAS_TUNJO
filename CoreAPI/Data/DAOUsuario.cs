using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
namespace Data
{
    public class DaoUsuario {
        private readonly Mapeo db = new Mapeo();

        /*
            * Autor: Jhonattan Pulido
            * Descripción: Servicio que funciona para traer los estados que estan registrados pero no han sido verificados por el administrador
            * Parámetros: Ninguno
            * Retorna: Lista de usuarios filtrados por verificación de cuenta en falso
        */
        public List<UUsuario> LeerUsuariosNoVerificados() {

            try {

                using (this.db) {

                    return this.db.Usuarios.Where(x => x.VerificacionCuenta == false).ToList();
                }

            } catch { throw; }

        }

        public List<UUsuario> ObtenerUsuarios() {

            using (var db = new Mapeo()) {
                try {
                    return db.Usuarios
                             .OrderByDescending(x => x.Id)
                             .ToList();
                } catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool Agregar(UUsuario usuarios)
        {
            try
            {
                db.Usuarios.Add(usuarios);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UUsuario Buscar(int id)
        {
            try
            {
                return db.Usuarios.Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Actualizar(int id, UUsuario usuario)
        {
            try
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Existe(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                var usuarios = db.Usuarios.Find(id);
                db.Usuarios.Remove(usuarios);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Existe(int id)
        {
            return db.Usuarios.Any(x => x.Id == id);
        }

        //public bool buscarCedula(double cedula)
        public bool buscarCedula(string cedula)
        {
            using (var db = new Mapeo())
            {

                try
                {

                    return db.Usuarios.Where(x => x.NumeroDocumento.Equals(cedula)).ToList().Count() > 0 ? false : true;

                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }
        }


        public bool buscarCorreo(string correo)
        {
            using (var db = new Mapeo())
            {

                try
                {

                    return db.Usuarios.Where(x => x.CorreoElectronico.Equals(correo)).ToList().Count() > 0 ? false : true;

                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }
        }
        /*
       @Autor : Carlos Alfonso Pinilla Garzón
       *Fecha de creación: 13/03/2020
       *Descripcion: Metodo para cambiar estado de la cuenta del ususario
       *Este metodo recibe: 
       *Retorna:
       */
        public void cambiarEstado(string cedula)
        {
            using (var db = new Mapeo())
            {
                var user = db.Usuarios.Where(x => x.NumeroDocumento.Equals(cedula)).FirstOrDefault();
                if (user.EstadoCuenta == false)
                {
                    user.EstadoCuenta = true;
                }
                else
                {
                    user.EstadoCuenta = false;
                }

                db.SaveChanges();
            }
        }
        /**
        *@Autor : Gabriel Andres Zapata Morera
        *Fecha de creación: 14/04/2020
        *Descripcion: Metodo para cambiar estado de la cuenta del ususario segun el estadoFiltro
        *Este metodo recibe:  estadoFiltro,  id_Usuario
        *Retorna: string validacion
        */
        public string CambiarEstado_Usuarios(int estadoFiltro, int id_Usuario)
        {
            using (var db = new Mapeo())
            {
                string validacion = "";
                var user = db.Usuarios.Where(x => x.Id == id_Usuario).FirstOrDefault();
                if (estadoFiltro == 1)
                {
                    user.EstadoCuenta = false;
                    validacion = "Usuario Deshabilitado Satisfactoriamente";
                    db.SaveChanges();
                    return validacion;
                }
                else if (estadoFiltro == 2)
                {
                    user.EstadoCuenta = true;
                    validacion = "Usuario Habilitado Satisfactoriamente";
                    db.SaveChanges();
                    return validacion;
                }
                return validacion;

            }
        }
        /**
        *@Autor : Carlos Alfonso Pinilla Garzón
        *Fecha de creación: 21/04/2020
        *Descripcion: Metodo para obtener los roles
        *Este metodo recibe: nada
        *Retorna: lista de roles
        */
        public List<URol> ObtenerRoles()
        {
            try
            {
                return db.Roles.ToList();
            }catch(Exception ex)
            {
                throw ex;
            }
        }


    }
}
