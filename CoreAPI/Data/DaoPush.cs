using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Utilitarios;
namespace Data
{

    public class DaoPush
    {


        private readonly Mapeo db = new Mapeo();


        public bool insertarPush( UPush push) {
            try {
                push.Fecha = DateTime.Now;
               
                if (db.Push.Where(x => x.ObjetoPush == push.ObjetoPush).Count() == 0){
                    db.Push.Add(push);
                    db.SaveChanges();
                    return true;
                }
                else {
                    //si no es cero validar el id que no sea cero  y actualizarlo 
                    var pushed = db.Push.Where(x => x.ObjetoPush == push.ObjetoPush).FirstOrDefault();
                    if (pushed.UserId == 0){
                        //moficarlo
                        pushed.UserId = push.UserId;
                        db.SaveChanges();
                        return true;
                    }
                    else { return false; }
                    
                }
            }catch (Exception ) {
                return false; 
            }
        }



        public IEnumerable<UPush> Tokensnotificaciones() {

            try{
                return db.Push.Where(x => x.EstadoId == 1);
            }catch (Exception ex) { throw ex; }
  
        }

        public string obtenerTokenUsuario(int id) {
            var push = db.Push.Where(x => x.UserId == id).FirstOrDefault();
            return push.ObjetoPush;

        }

    }
}
