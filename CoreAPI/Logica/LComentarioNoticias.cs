using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Newtonsoft.Json;
using Utilitarios;
namespace Logica
{
    public class LComentarioNoticias
    {


        public bool agregarComentarioNoticia(UComentarioNoticia comentarioNotica) {
            return new DaoComentariosNoticias().agregarComentarioNoticia(comentarioNotica);
        }

        

        public IEnumerable<UComentarioNoticia> listaComentariosNoticia(int noticiaId)
        {
            return new DaoComentariosNoticias().ListaComentariosNoticia(noticiaId);
        }


        /// noticias + comentarios
   

        public IEnumerable<UNoticia> enviarNoticiaComentarios() {
            List<UNoticia> noticias = new DaoComentariosNoticias().enviarNoticias();
            if (noticias.Count() < 0)
            {
                return null;
            }
            else {
                for (int i = 0; i < noticias.Count; i++){
                    if (!String.IsNullOrEmpty(noticias[i].ImagenesUrl)) {

                        noticias[i].ListaImagenes = JsonConvert.DeserializeObject<List<string>>(noticias[i].ImagenesUrl);
                    }
                    List<UComentarioNoticia> listaComentariosNoticia = new DaoComentariosNoticias().ListaComentariosNoticia(noticias[i].Id).OrderByDescending(x => x.FechaPublicacion).ToList();
                    noticias[i].ListaNoticias = listaComentariosNoticia;
                }
            }
            return noticias.OrderByDescending(x => x.FechaPublicacion).ToList() ;
        }




        public UNoticia enviarVerNoticia(int id)
        {
            UNoticia noticias = new DaoComentariosNoticias().enviarNoticias().Where(x => x.Id == id ).FirstOrDefault();
            if (noticias==null){
                return null;
            }
            else{                
                    if (!String.IsNullOrEmpty(noticias.ImagenesUrl)){
                        noticias.ListaImagenes = JsonConvert.DeserializeObject<List<string>>(noticias.ImagenesUrl);
                    }
                    List<UComentarioNoticia> listaComentariosNoticia = new DaoComentariosNoticias().ListaComentariosNoticia(noticias.Id);
                    noticias.ListaNoticias = listaComentariosNoticia;
            }
            return noticias;
        }

        public bool reportarComentarioNoticia(long idComentario)
        {
            return new DaoComentariosNoticias().reportarComentarioNotica(idComentario);
        }

        public bool eliminarComentarioNoticia(long idComentario)
        {
            return new DaoComentariosNoticias().eliminarComentarioNoticia(idComentario);
        }


    }
}
