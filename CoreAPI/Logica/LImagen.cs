using Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logica
{
    public class LImagen
    {
        public object UploadImages(HttpRequest request, string carpeta)
        {
            string messageError = string.Empty;
            try
            {
                if (request.Files.Count > 0)
                {
                    foreach(string key in request.Files.Keys)
                    {
                        var postedFile = request.Files.Get(key);
                        string extension = Path.GetExtension(postedFile.FileName);
                        if ( ValidExtension(extension) )
                        {
                            bool upload = new DaoImagen().UploadImage(postedFile, carpeta);
                            if (!upload)
                            {
                                messageError = $"Ha ocurrido un error subiendo la imagen { postedFile.FileName }";
                                break;
                            }
                        } else
                        {
                            messageError = $"El archivo { postedFile.FileName } NO es una imagen";
                            break;
                        }
                    }
                } else
                {
                    messageError = "No has enviado ningún archivo";
                }
            } catch(Exception ex)
            {
                messageError = ex.Message;
            }

            if (!string.IsNullOrEmpty(messageError))
            {
                return new { ok = false, message = messageError };
            } else
            {
                return new { ok = true, message = "Upload images" };
            }
        }

        private bool ValidExtension(string extension)
        {
            return (extension == ".jpg" || extension == ".jpeg" || extension == ".png");
        }
    }
}
