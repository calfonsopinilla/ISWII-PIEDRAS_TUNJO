using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("estado_pqr", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UEstadoPQR {

        // Variables
        private int id;
        private string nombre;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }        
        [Column("nombre")]
        public string Nombre { get => nombre; set => nombre = value; }
    }
}
