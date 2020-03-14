using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("usuario", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UUsuario {

        // Variables
        private int id; // Guarda el id de cada usuario
        private string nombre; // Guarda el nombre de el usuario
        private string apellido; // Guarda el apellido de el usuario
        private string tipoDocumento; // Guarda el tipo de documento TI, CC, CE
        private double numeroDocumento; // Guarda el numero de documento de el usuario
        private string lugarExpedicion; // Guarda el lugar de expedición del documento de identidad
        private string correoElectronico; // Guarda el correo electronico
        private string clave; // Guarda la clave del usuario para iniciar sesion
        private string icono_url; // Guarda la dirección url del icono seleccionado por el usuario
        private bool verificacionCuenta; // Estado inicial en false que significa que el usuario creo una cuenta pero debe ser verificada por el administrador para que se active satisfactoriamente su cuenta
        private bool estadoCuenta; // Estado inicial en true que significa que el usuario esta activo, de lo contrario si su cuenta esta desactivada estara en estado false
        private int rolId; // Se guarda el Id del rol de los usuarios
        private string rolNombre; // Se guarda el nombre del rol dependiendo del Id del rol
        private string imagen_documento;
        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("nombre")]
        public string Nombre { get => nombre; set => nombre = value; }
        [Column("apellido")]
        public string Apellido { get => apellido; set => apellido = value; }
        [Column("tipo_documento")]
        public string TipoDocumento { get => tipoDocumento; set => tipoDocumento = value; }
        [Column("numero_documento")]
        public double NumeroDocumento { get => numeroDocumento; set => numeroDocumento = value; }
        [Column("lugar_expedicion")]
        public string LugarExpedicion { get => lugarExpedicion; set => lugarExpedicion = value; }
        [Column("correo_electronico")]
        public string CorreoElectronico { get => correoElectronico; set => correoElectronico = value; }
        [Column("clave")]
        public string Clave { get => clave; set => clave = value; }
        [Column("icono_url")]
        public string Icono_url { get => icono_url; set => icono_url = value; }
        [Column("verificacion_cuenta")]
        public bool VerificacionCuenta { get => verificacionCuenta; set => verificacionCuenta = value; }
        [Column("estado_cuenta")]
        public bool EstadoCuenta { get => estadoCuenta; set => estadoCuenta = value; }
        //[ForeignKey("RolId")]
        [Column("rol_id")]
        public int RolId { get => rolId; set => rolId = value; }
        [Column("imagen_documento")]
        public string Imagen_documento { get => imagen_documento; set => imagen_documento = value; }
        [NotMapped]
        public string RolNombre { get => rolNombre; set => rolNombre = value; }
      
    }
}
