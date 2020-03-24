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
    public class DaoPqr
    {
        public IEnumerable<UpqrInformacion> listaPqr() {

            using (var db = new Mapeo())
            {
                try
                {
                    List<UpqrInformacion> informacion = new List<UpqrInformacion>();
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
                    informacion = lista.AsEnumerable().Select(p => new UpqrInformacion()
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

        public bool AgregarPqr(UPQR pqr) {

            using (var db = new Mapeo()){
                try{
                    db.pqr.Add(pqr);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {   
                    throw ex;
                }
            }
        }
        public bool actualizarPqr(int id, UPQR pqr) {

            using (var db = new Mapeo())
            {
                try
                {
                    db.Entry(pqr).State = EntityState.Modified;
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
        }


        public bool eliminarPqr(int id) {

            using (var db = new Mapeo()){
                try
                {
                    var pqr = db.pqr.Find(id);
                    db.pqr.Remove(pqr);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex) {
                    throw ex;
                }
            }
        }



        public bool Existe(int id)
        {
            using (var db = new Mapeo()) { 
                return db.pqr.Any(x => x.Id == id);
            }
        }

        public UpqrInformacion BuscarPqr(int id)
        {
            using (var db = new Mapeo())
            {
                UpqrInformacion informacion = new UpqrInformacion();
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
                             }).Where(x => x.Id ==id ).First();
                informacion.Id = lista.Id;
                informacion.Pregunta = lista.Pregunta;
                informacion.Respuesta = lista.Respuesta;
                informacion.FechaPublicacion = lista.FechaPublicacion;
                informacion.Estado = lista.Estado;
                informacion.EstadoIdU = lista.EstadoId;
                informacion.EstadoPqrU1 = lista.EstadoPQR;
                informacion.NombreUsuario = lista.NombreUsuario;
                informacion.ApellidoUsuarioU1 = lista.ApellidoUsuario;
                return informacion;

            }
        }


    }
}
