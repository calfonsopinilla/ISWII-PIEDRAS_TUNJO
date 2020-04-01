using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable]
    [Table("tokens_correo_electronico", Schema = "security")]
    public class UTokenCorreo {

        private long id;
        private string token;
        private string correoElectronico;
        private DateTime fechaGeneracion;
        private DateTime fechaVencimiento;
        private DateTime fechaNacimiento;
        private int aplicacionId;
        private string nombre;
        private string apellido;
        private string clave;
        private string tipoDocumento;
        private string numeroDocumento;
        private string iconoUrl;
        private int rolId;

        [Key]
        [Column("id")]
        public long Id { get => id; set => id = value; }
        [Column("token")]
        public string Token { get => token; set => token = value; }
        [Column("correo_electronico")]
        public string CorreoElectronico { get => correoElectronico; set => correoElectronico = value; }
        [Column("fecha_generacion")]
        public DateTime FechaGeneracion { get => fechaGeneracion; set => fechaGeneracion = value; }
        [Column("fecha_vencimiento")]
        public DateTime FechaVencimiento { get => fechaVencimiento; set => fechaVencimiento = value; }
        [Column("aplicacion_id")]
        public int AplicacionId { get => aplicacionId; set => aplicacionId = value; }
        [Column("nombre")]
        public string Nombre { get => nombre; set => nombre = value; }
        [Column("apellido")]
        public string Apellido { get => apellido; set => apellido = value; }
        [Column("clave")]
        public string Clave { get => clave; set => clave = value; }
        [Column("tipo_documento")]
        public string TipoDocumento { get => tipoDocumento; set => tipoDocumento = value; }
        [Column("numero_documento")]
        public string NumeroDocumento { get => numeroDocumento; set => numeroDocumento = value; }
        [Column("icono_url")]
        public string IconoUrl { get => iconoUrl; set => iconoUrl = value; }
        [Column("rol_id")]
        public int RolId { get => rolId; set => rolId = value; }
        [Column("fecha_nacimiento")]
        public DateTime FechaNacimiento { get => fechaNacimiento; set => fechaNacimiento = value; }
    }
}
