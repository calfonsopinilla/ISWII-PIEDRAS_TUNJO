using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios
{
    /*[Serializable]// Se puede expresar en formato JSON
    [Table("usuario", Schema = "usuario")] //Tabla y esquema a relacionar
    */
    public class URegistroUser
    {
        //variables
        private string nombre; // guarda el valor del nombre del usuario
        private string apellido; // guarda el valor del apellido del usuario
        private string numDocumento; // guarda el valor del documento del usuario
        private string tipoDocumento; // guarda el valor del tipo de documento del usuario
        private string lugarExpedicion; // guarda el valor del lugar de expedicion del documento del usuario
        private string correoElectronico; // guarda el valor del correo electronico del usuario
        private string clave; // guarda el valor de la clave de acceso a la cuenta del usuario
        private string iconoUrl; // guarda la url del icono del usuario
        private Boolean cedulaExistente; //guarda el valor booleano proveniente de la consulta de la cedula existente 
        private Boolean emailExistente; //guarda el valor booleano proveniente de la consulta del correo existente 


        /**
         * Metodos get y set de las variables
         **/
        public bool CedulaExistente { get => cedulaExistente; set => cedulaExistente = value; }
        public bool EmailExistente { get => emailExistente; set => emailExistente = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string NumDocumento { get => numDocumento; set => numDocumento = value; }
        public string TipoDocumento { get => tipoDocumento; set => tipoDocumento = value; }
        public string LugarExpedicion { get => lugarExpedicion; set => lugarExpedicion = value; }
        public string CorreoElectronico { get => correoElectronico; set => correoElectronico = value; }
        public string Clave { get => clave; set => clave = value; }
        public string IconoUrl { get => iconoUrl; set => iconoUrl = value; }
    }
}
