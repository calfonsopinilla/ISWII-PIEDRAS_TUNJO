using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("informacion_parque", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UInformacionParque {

        // Variables
        private int id;
        private string property;
        private string descripcion;
        private string imagenesUrl;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("property")]
        public string Property { get => property; set => property = value; }
        [Column("descripcion")]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        [Column("imagenes_url")]
        public string ImagenesUrl { get => imagenesUrl; set => imagenesUrl = value; }
    }
}
