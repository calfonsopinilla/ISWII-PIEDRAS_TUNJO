using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    
    [Table("evento", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UEvento {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("fecha_publicacion")]
        public DateTime FechaPublicacion { get; set; } = DateTime.Now;
        [Column("fecha")]
        public DateTime Fecha { get; set; }
        [Column("descripcion")]
        public string Descripcion { get; set; }
        [Column("calificacion")]
        public double Calificacion { get; set; }
        [Column("imagenes_url")]
        public string ImagenesUrl { get; set; }
        [Column("comentarios_id")]
        public string ComentariosId { get; set; }
        [Column("token")]
        public string Token { get; set; } = "";
        [NotMapped]
        public List<UComentarioEvento> ListaComentariosEvento { get; set; }
    }
}
