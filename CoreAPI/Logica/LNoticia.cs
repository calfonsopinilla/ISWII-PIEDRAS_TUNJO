using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using Data;
using Newtonsoft.Json;

namespace Logica
{
    public class LNoticia
    {
        /*
      @Autor: Carlos Alfonso Pinilla Garzon
      *Fecha de creación: 18/03/2020
      *Descripcion:
      *Recibe:
      *Retorna:
      */
        public List<UNoticia> ObtenerNoticias()
        {
            return new DAONoticia().ObtenerNoticias();
        }
        /*
       @Autor: Carlos Alfonso Pinilla Garzon
       *Fecha de creación: 18/03/2020
       *Descripcion: 
       *Recibe: 
       *Retorna: 
       */
        public bool AgregarNoticia(UNoticia noticia)
        {
            noticia.Token = "";
            return new DAONoticia().AgregarNoticia(noticia);
        }



        public UNoticia Buscar(int id)
        {
            return new DAONoticia().Buscar(id);
        }


        public bool Actualizar(int id, UNoticia noticia)
        {
            noticia.Token = "";
            return new DAONoticia().Actualizar(id, noticia);
        }
        /*
       @Autor: Carlos Alfonso Pinilla Garzon
       *Fecha de creación: 18/03/2020
       *Descripcion: 
       *Recibe: 
       *Retorna: 
       */
        public bool EliminarNoticia(int id)
        {
            try
            {
                return new DAONoticia().EliminarNoticia(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
