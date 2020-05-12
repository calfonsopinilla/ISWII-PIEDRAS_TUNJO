using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
namespace Data
{
    public class DaoToken
    {

        public void insertarToken(UToken token) {
            using (var db = new Mapeo()) {

                try{
                    db.token.Add(token);
                    db.SaveChanges();
                }
                catch (Exception ex) {
                    throw ex;
                }
            }
        }
        public UToken obtenerTokenUser(int  id) {

            using (var db = new Mapeo()){
                try
                {
                    var token = db.token.Where(x => x.UserId == id).FirstOrDefault();
                    return token;
                }
                catch (Exception ex){
                    return null;
                    throw ex;
                }
            }


            
        }



    }
}
