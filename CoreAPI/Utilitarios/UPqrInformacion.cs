using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitarios
{
    public class UPqrInformacion
    {


        private int id;
        private string pregunta;
        private string respuesta;
        private DateTime fechaPublicacion;
        private bool estado; 
        private string nombreUsuario;
        private string EstadoPqrU;
        private int estadoIdU;
        private string ApellidoUsuarioU;

        public int Id { get => id; set => id = value; }
        public string Pregunta { get => pregunta; set => pregunta = value; }
        public string Respuesta { get => respuesta; set => respuesta = value; }
        
        
        public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
        
        public DateTime FechaPublicacion { get => fechaPublicacion; set => fechaPublicacion = value; }
        public string EstadoPqrU1 { get => EstadoPqrU; set => EstadoPqrU = value; }
        public string ApellidoUsuarioU1 { get => ApellidoUsuarioU; set => ApellidoUsuarioU = value; }
        public bool Estado { get => estado; set => estado = value; }
        public int EstadoIdU { get => estadoIdU; set => estadoIdU = value; }
    }
}
