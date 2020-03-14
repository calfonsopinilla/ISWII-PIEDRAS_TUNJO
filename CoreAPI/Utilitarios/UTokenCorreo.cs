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
        private int aplicacionId;

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
    }
}
