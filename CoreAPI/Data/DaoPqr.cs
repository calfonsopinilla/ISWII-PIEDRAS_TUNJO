using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
namespace Data
{
    public class DaoPqr
    {


        /*
        @Autor : Jose Luis Soriano Roa
        *Fecha de creación: 19/03/2020
        *Descripcion : metodo que envia la informacion de los pqr
        *Este metodo recibe : No resive parametros
        * Retorna: lista de la informacion delos pqr un objeto de tipo UPqrInformacion 
        */
        public List<UPqrInformacion> informacionPqr()
        {
            using (var db = new Mapeo())
            {
                try
                {

                    List<UPqrInformacion> informacion = new List<UPqrInformacion>();
                    var lista = (from pqr in db.pqr
                                 join Epqr in db.EstadoPqr on pqr.EstadoId equals Epqr.Id
                                 join uu in db.Usuarios on pqr.UsuarioId equals uu.Id
                                 select new
                                 {
                                     Id = pqr.Id,
                                     Pregunta = pqr.Pregunta,
                                     Respuesta = pqr.Respuesta,
                                     FechaPublicacion = pqr.FechaPublicacion,
                                     Estado = pqr.Estado,
                                     EstadoPQR = Epqr.Nombre,
                                     EstadoId = pqr.EstadoId,
                                     NombreUsuario = uu.Nombre,
                                     ApellidoUsuario = uu.Apellido,
                                     
                                 }).ToList();
                    informacion = lista.AsEnumerable().Select(p => new UPqrInformacion()
                    {
                        Id = p.Id,
                        Pregunta = p.Pregunta,
                        Respuesta = p.Respuesta,
                        FechaPublicacion = p.FechaPublicacion,
                        ApellidoUsuarioU1 = p.ApellidoUsuario,
                        EstadoPqrU1 = p.EstadoPQR,
                        Estado = p.Estado,
                        NombreUsuario = p.NombreUsuario,
                        EstadoIdU = p.EstadoId
                       
                    }).ToList();

                    return informacion;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        /*
      @Autor : Jose Luis Soriano Roa
      *Fecha de creación: 19/03/2020
      *Descripcion : metodo que cambia el estado de la pqr-- 
      *Este metodo recibe : resive el id de la pqr  ....
      * Retorna: true si el objeto se modifico , false si el registro su estado ya esta en falso
      */

        public string cambiarEstado(int id)
        {
            using (var db = new Mapeo())
            {
                try
                {
                    var registro = db.pqr.Where(x => x.Id.Equals(id) && x.Estado == true).FirstOrDefault();
                    if (registro != null)
                    {
                        registro.Estado = false;
                        db.SaveChanges();
                        return "true";
                    }
                    else
                    {
                        return "false";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
        /*
    @Autor : Jose Luis Soriano Roa
    *Fecha de creación: 19/03/2020
    *Descripcion : metodo que inserta un pqr-- 
    *Este metodo recibe : Un objeto de tipo UPqr ....
    * Retorna: true si se inserto con exito , false si no fue posible insertar el registro
    */


        public void agregarPqr(UPQR  pqr)
        {
            using (var db = new Mapeo())
            {
                try
                {
                    db.pqr.Add(pqr);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    //return "false";
                    throw ex;
                    
                }

            }
        }


        /*
       @Autor : Jose Luis Soriano Roa
       *Fecha de creación: 19/03/2020
       *Descripcion : metodo que modifica pqr-- 
       *Este metodo recibe : resive objeto de tipo upqr
       * Retorna: true si el objeto se modifico , false si el registro su estado ya esta en falso
       */

        public string editarPqr(UPQR pqr1)
        {
            using (var db = new Mapeo())
            {
                try
                {

                    var registro = db.pqr.Where(x => x.Id.Equals(pqr1.Id)).FirstOrDefault();

                    if (registro != null)
                    {

                        registro.Pregunta = pqr1.Pregunta;
                        registro.Respuesta = pqr1.Respuesta;
                        registro.Token = pqr1.Token;
                        registro.UsuarioId = pqr1.UsuarioId;
                        registro.UltimaModificacion = pqr1.UltimaModificacion;
                        registro.FechaPublicacion = pqr1.FechaPublicacion;
                        registro.Estado = pqr1.Estado;
                        registro.EstadoId = pqr1.EstadoId;
                        db.SaveChanges();
                        return "true";

                    }
                    else {

                        return "false";
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }



    }
}
