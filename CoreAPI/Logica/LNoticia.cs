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
        public string informacionNoticia()
        {
            try
            {
                List<UNoticia> noticias = new DAONoticia().informacionNoticia();
                if (noticias.Count < 0)
                {
                    return "null";
                }
                else
                {
                    for (int x = 0; x < noticias.Count(); x++)
                    {
                        if (!String.IsNullOrEmpty(noticias[x].ImagenesUrl))
                        {
                            noticias[x].ListaImagenes = JsonConvert.DeserializeObject<List<string>>(noticias[x].ImagenesUrl);
                        }
                    }
                    return JsonConvert.SerializeObject(noticias);
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
        public bool agregarNoticia(string datosJson)
        {
            try
            {
                UNoticia datos = JsonConvert.DeserializeObject<UNoticia>(datosJson);
                if (new DAONoticia().buscarId(datos.Id) != true)
                {
                    DAONoticia noticia = new DAONoticia();
                    noticia.agregarNoticias(datos);
                    return true;
                }
                else
                {
                    return false;
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
