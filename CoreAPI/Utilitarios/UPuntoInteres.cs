using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitarios
{
    [Table("punto_interes", Schema = "parque")]
    public class UPuntoInteres
    {
        [Key]
        [Column("if")]
        public int Id { get; set; }
        [Column("descripcion")]
        public string Descripcion { get; set; }
        [Column("latitud")]
        public string Latitud { get; set; }
        [Column("longitud")]
        public string Longitud { get; set; }
        [Column("token")]
        public string Token { get; set; }
        [Column("last_modification")]
        public DateTime LastModification { get; set; }
    }
}
