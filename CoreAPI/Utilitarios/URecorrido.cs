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
        [Column("ruta", TypeName = "geometry")]
        public string Ruta { get; set; }
        [Column("puntos_interes")]
        public string PuntosInteres { get; set; }
        [Column("tiempo_estimado")]
        public int TiempoEstimado { get; set; }
    }
}
