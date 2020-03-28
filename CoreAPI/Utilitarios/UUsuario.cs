using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Table("usuario", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UUsuario {

        // Variables
        private int id; // Guarda el id de cada usuario
        private string nombre; // Guarda el nombre de el usuario
        private string apellido; // Guarda el apellido de el usuario
        private string tipoDocumento; // Guarda el tipo de documento TI, CC, CE
        private string numeroDocumento; // Guarda el numero de documento de el usuario
        private string lugarExpedicion; // Guarda el lugar de expedición del documento de identidad
        private string correoElectronico; // Guarda el correo electronico
        private string clave; // Guarda la clave del usuario para iniciar sesion
        private string icono_url; // Guarda la dirección url del icono seleccionado por el usuario
        private bool verificacionCuenta; // Estado inicial en false que significa que el usuario creo una cuenta pero debe ser verificada por el administrador para que se active satisfactoriamente su cuenta
        private bool estadoCuenta; // Estado inicial en true que significa que el usuario esta activo, de lo contrario si su cuenta esta desactivada estara en estado false
        private int rolId; // Se guarda el Id del rol de los usuarios
        private string rolNombre; // Se guarda el nombre del rol dependiendo del Id del rol
        private string imagen_documento;
        private string token;

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("apellido")]
        public string Apellido { get; set; }
        [Column("tipo_documento")]
        public string TipoDocumento { get; set; }
        [Column("numero_documento")]
        public string NumeroDocumento { get; set; }
        [Column("lugar_expedicion")]
        public string LugarExpedicion { get; set; }
        [Column("correo_electronico")]
        public string CorreoElectronico { get; set; }
        [Column("clave")]
        public string Clave { get; set; }
        [Column("icono_url")]
        public string Icono_url { get; set; }
        [Column("verificacion_cuenta")]
        public bool VerificacionCuenta { get; set; } = false;
        [Column("estado_cuenta")]
        public bool EstadoCuenta { get; set; } = false;
        [Column("rol_id")]
        public int RolId { get; set; }
        [Column("imagen_documento")]
        public string Imagen_documento { get; set; }
        [Column("token")]
        public string Token { get; set; } = "";

        [NotMapped]
        public string RolNombre { get; set; }
       
    }
}
