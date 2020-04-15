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
                if (noticias.Count < 0)
                {
                    return null;
                }
                else
                {
                    for (int x = 0; x < noticias.Count(); x++)
                    {
                        if (!String.IsNullOrEmpty(noticias[x].ImagenesUrl))
                        {
                            noticias[x].ListaImagenes = JsonConvert.DeserializeObject<List<string>>(noticias[x].ImagenesUrl);
                            noticias[x].FechaPublicacion = DateTime.Parse(noticias[x].FechaPublicacion.ToString("dd/MM/yyyy"));
                            noticias[x].Fecha_Publicacion = noticias[x].FechaPublicacion.ToString("dd/MM/yyyy");
                        }
                    }
                    //return JsonConvert.SerializeObject(noticias);
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
            List<string> lista = new List<string>();
            lista.Add(noticia.ImagenesUrl);
            noticia.ImagenesUrl = JsonConvert.SerializeObject(lista);
            noticia.Token = "";
            noticia.Estado = 1;
            noticia.FechaPublicacion = DateTime.Now;
            return new DAONoticia().agregarNoticias(noticia);
        }



        public UNoticia Buscar(int id)
        {
            return new DAONoticia().Buscar(id);
        }


        public bool Actualizar(int id, UNoticia noticia)
        {
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
