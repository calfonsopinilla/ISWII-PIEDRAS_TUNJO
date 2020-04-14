using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Table("usuario", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UUsuario {

        private string controlCuenta;


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
        [Column("fecha_nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [NotMapped]
        public string RolNombre { get; set; }

    }
}
