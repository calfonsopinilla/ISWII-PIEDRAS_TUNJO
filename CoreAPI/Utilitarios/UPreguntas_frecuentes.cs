using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios
{
    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("preguntas_frecuentes", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public  class UPreguntas_frecuentes
    {
        // Variables
        private int id;
        private string nombre;
        private string descripcion;
        

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("nombre")]
        public string Nombre { get => nombre; set => nombre = value; }
        [Column("descripcion")]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        [Column("token")]
        public string Token { get; set; }
        
    }
}
