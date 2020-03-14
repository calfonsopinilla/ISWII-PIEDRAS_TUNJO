using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data
{



        /*
        @Autor : Jose Luis Soriano Roa
        *Fecha de creación: 11/03/2020
        *Descripcion : Metodo que trae la lista de la informacion de los usuarios del sistema 
        *Este metodo recibe : No resive parametros
        * Retorna: lista de la informacion de los usuarios del sistema 
        */


    public class DaoAdministradorUsuario{

        public List<UUsuario> informacionUsuarios(){

            using (var db = new Mapeo()){
                try{
                    return db.Usuarios.ToList();
                }catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /*
       @Autor : Jose Luis Soriano Roa
       *Fecha de creación: 11/03/2020
       *Descripcion : metodo que agregar usuario
       *Este metodo recibe : Resive el objeto de tipo UUsuario
       * Retorna: por definir
       */

        public void agregarUsuario(UUsuario usuario)
        {
            using (var db = new Mapeo()) {

                try {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                }
                catch (Exception ex) {
                    throw ex;

                }

            }
        }




        public bool buscarCedula(double cedula)
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


    }


   

}
