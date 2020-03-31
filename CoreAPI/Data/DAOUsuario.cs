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
    public class DaoUsuario{
        private readonly Mapeo db = new Mapeo();

        public List<UUsuario> ObtenerUsuarios(){

            using (var db = new Mapeo()){
                try{
                    return db.Usuarios.ToList();
                }catch (Exception ex)
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

        public bool Actualizar(int id, UUsuario Usuarios)
        {
            try
            {
                db.Entry(Usuarios).State = EntityState.Modified;
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

    }
}
