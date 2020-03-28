using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("comentario_noticia", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UComentarioNoticia : UComentario {

        // Variables
        private int noticiaId;
        
        [ForeignKey("NoticiaId")]
        [Column("noticia_id")]
        public int NoticiaId { get => noticiaId; set => noticiaId = value; }
    }
}
