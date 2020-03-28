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

        public IEnumerable<UPQR> ListaPQR() {

            using (var db = new Mapeo())
            {
                try
                {
                    var pqrs = db.PQR
                                 .Include("UUsuario")
                                 .Include("UEstadoPQR")
                                 .OrderByDescending(x => x.Id)
                                 .ToList(); // Lazy Loading para traer los datos del usuario
                    return pqrs;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public IEnumerable<UPQR> ObtenerPorUsuario(int user_id)
        {
            using (var db = new Mapeo())
            {
                try
                {
                    var pqrs = db.PQR
                                 .Include("UUsuario")
                                 .Include("UEstadoPQR")
                                 .Where(x => x.UUsuarioId == user_id)
                                 .OrderByDescending(x => x.Id)
                                 .ToList();
                    return pqrs;
                }catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool AgregarPqr(UPQR pqr) {

            using (var db = new Mapeo()){
                try{
                    db.PQR.Add(pqr);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {   
                    throw ex;
                }
            }
        }
        public bool actualizarPqr(int id, UPQR PQR) {

            using (var db = new Mapeo())
            {
                try
                {
                    db.Entry(PQR).State = EntityState.Modified;
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
                    var PQR = db.PQR.Find(id);
                    db.PQR.Remove(PQR);
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
                return db.PQR.Any(x => x.Id == id);
            }
        }

        public UPQR BuscarPqr(int id)
        {
            using (var db = new Mapeo())
            {
                var pqrs = db.PQR
                             .Include("UUsuario")
                             .Include("UEstadoPQR")
                             .ToList();
                return pqrs.Find(x => x.Id == id);
            }
        }


    }
    
    }
