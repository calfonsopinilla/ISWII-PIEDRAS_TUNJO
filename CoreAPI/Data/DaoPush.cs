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

            try{
                push.Fecha = DateTime.Now;
                db.Push.Add(push);
                db.SaveChanges();
                return true;
            }catch (Exception ex) {
                return false;
            }

        }


    }
}
