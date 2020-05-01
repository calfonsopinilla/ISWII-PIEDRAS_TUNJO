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
                if (db.Push.Where(x => x.ObjetoPush == push.ObjetoPush).Count() == 0)
                {
                    db.Push.Add(push);
                    db.SaveChanges();
                    return true;
                }
                else {
                    return false;
                }
            }catch (Exception ex) {
                return false; 
            }
        }

        public IEnumerable<UPush> Tokensnotificaciones() {

            try{
                return db.Push.Where(x => x.EstadoId == 1);
            }catch (Exception ex) { throw ex; }
   
        }


    }
}
