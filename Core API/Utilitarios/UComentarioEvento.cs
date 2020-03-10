using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("comentario_evento", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UComentarioEvento : UComentario {

        // Variables
        private int eventoId;

        [ForeignKey("EventoId")]
        [Column("evento_id")]
        public int EventoId { get => eventoId; set => eventoId = value; }
    }
}
