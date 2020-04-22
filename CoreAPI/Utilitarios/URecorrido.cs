using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios
{
    [Table("recorrido", Schema = "parque")]
    public class URecorrido
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("ruta")]
        public string Ruta { get; set; }
        [Column("ruta_text")]
        public string RutaText { get; set; }
        [Column("puntos_interes")]
        public string PuntosInteres { get; set; }
        [Column("puntos_control")]
        public string PuntosControl { get; set; }
        [Column("tiempo_estimado")]
        public int TiempoEstimado { get; set; }
        [Column("token")]
        public string Token { get; set; }
        [Column("last_modification")]
        public DateTime LastModification { get; set; }
    }
}
