using System;
using System.Collections.Generic;
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
        private string token;
        private DateTime lastModification;
        private List<string> listaImagenesUrl;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("property")]
        public string Property { get => property; set => property = value; }
        [Column("descripcion")]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        [Column("imagenes_url")]
        public string ImagenesUrl { get => imagenesUrl; set => imagenesUrl = value; }
        [Column("token")]
        public string Token { get => token; set => token = value; }
        [Column("last_modification")]
        public DateTime LastModification { get => lastModification; set => lastModification = value; }
        [NotMapped]
        public List<string> ListaImagenesUrl { get => listaImagenesUrl; set => listaImagenesUrl = value; }
      
    }
}
