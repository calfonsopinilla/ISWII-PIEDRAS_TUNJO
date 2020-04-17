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
        public List<UNoticia> informacionNoticia()
        {
            try
            {
                List<UNoticia> noticias = new DAONoticia().informacionNoticia();
                if (noticias.Count < 0){
                    return null;
                }
                else{
                    return noticias;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*
       @Autor: Carlos Alfonso Pinilla Garzon
       *Fecha de creación: 18/03/2020
       *Descripcion: 
       *Recibe: 
       *Retorna: 
       */
        public bool agregarNoticia(UNoticia noticia)
        {
            noticia.Token = "";
            return new DAONoticia().agregarNoticias(noticia);
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
        public bool actualizarNoticia(string datosJson)
        {
            try
            {
                UNoticia datos = JsonConvert.DeserializeObject<UNoticia>(datosJson);
                DAONoticia noticia = new DAONoticia();
                noticia.actulizarNoticia(datos);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*
       @Autor: Carlos Alfonso Pinilla Garzon
       *Fecha de creación: 18/03/2020
       *Descripcion: 
       *Recibe: 
       *Retorna: 
       */
        public bool eliminarNoticia(int id)
        {
            try
            {
                DAONoticia noticia = new DAONoticia();
                noticia.eliminarNoticia(id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
