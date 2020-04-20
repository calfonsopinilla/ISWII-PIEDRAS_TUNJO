using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Table("pictograma", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UPictograma {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("descripcion")]
        public string Descripcion { get; set; }
        [Column("imagenes_url")]
        public string ImagenesUrl { get; set; }
        [Column("calificacion")]
        public double Calificacion { get; set; } = 0;
        [Column("estado")]
        public int Estado { get; set; } = 1;
        [Column("latitud")]
        public string Latitud { get; set; }
        [Column("longitud")]
        public string Longitud { get; set; }
    }
}
