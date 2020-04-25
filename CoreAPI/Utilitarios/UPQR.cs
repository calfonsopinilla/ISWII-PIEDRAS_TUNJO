using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Table("pqr", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UPQR {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("fecha_publicacion")]
        public DateTime FechaPublicacion { get; set; }
        [Column("pregunta")]
        public string Pregunta { get; set; }
        [Column("respuesta")]
        public string Respuesta { get; set; }
        [Column("token")]
        public string Token { get; set; } = "";
        [Column("last_modification")]
        public DateTime LastModification { get; set; } = DateTime.Now;                
        [Column("fecha_respuesta")]
        public DateTime FechaRespuesta { get; set; }
        [Column("estado_id")]
        public int UEstadoPQRId { get; set; }
        public UEstadoPQR UEstadoPQR { get; set; }

        [Column("usuario_id")]
        public int UUsuarioId { get; set; }
        public UUsuario UUsuario { get; set; }
    }
}
