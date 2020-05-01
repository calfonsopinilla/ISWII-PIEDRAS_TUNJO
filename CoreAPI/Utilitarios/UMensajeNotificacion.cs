using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitarios
{
    public class UMensajeNotificacion
    {
  
        string titulo;
        string descripcion;
        string tipo;

        public string Titulo { get => titulo; set => titulo = value; }
        
        public string Tipo { get => tipo; set => tipo = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
    }
}
