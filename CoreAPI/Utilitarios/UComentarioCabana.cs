using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("comentario_cabana", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UComentarioCabana {

        // Variables
        private int cabanaId;

        //[ForeignKey("CabanaId")]
        [Column("cabana_id")]
        public int CabanaId { get => cabanaId; set => cabanaId = value; }
    }
}
