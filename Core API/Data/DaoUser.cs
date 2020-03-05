using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data
{
    public class DaoUser
    {
        public List<UUser> GetUsers()
        {
            using (var db = new Mapeo())
            {
                return db.Usuarios.ToList();
            }
        }
    }
}
