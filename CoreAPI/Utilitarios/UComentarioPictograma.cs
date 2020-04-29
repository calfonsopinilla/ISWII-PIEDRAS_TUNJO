using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("comentario_pictograma", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UComentarioPictograma : UComentario {

        // Variables
        private int pictogramaId;
        
        [Column("pictograma_id")]
        public int PictogramaId { get; set; }
    }
}

