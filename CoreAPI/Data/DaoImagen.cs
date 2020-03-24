﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data
{
    public class DaoImagen
    {
        public bool UploadImage(HttpPostedFile postedFile, string carpeta)
        {
            try
            {
                string nameFile = postedFile.FileName.Replace(" ", "_");
                string path = HttpContext.Current.Server.MapPath($"~/Imagenes/{ carpeta }/{ nameFile }");
                postedFile.SaveAs(path);
                return true;
            }catch(Exception)
            {
                return false;
            }
        }
    }
}
