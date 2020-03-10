using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("pictograma", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UPictograma {

        // Variables
        private int id;
        private string nombre;
        private double calificaion;
        private string descripcion;
        private List<string> imagenesUrl;
        private List<string> comentariosId;
        private List<UComentarioPictograma> listaComentariosPictograma;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("nombre")]
        public string Nombre { get => nombre; set => nombre = value; }
        [Column("calificacion")]
        public double Calificaion { get => calificaion; set => calificaion = value; }
        [Column("descripcion")]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        [Column("imagenes_url")]
        public List<string> ImagenesUrl { get => imagenesUrl; set => imagenesUrl = value; }
        [Column("comentarios_id")]
        public List<string> ComentariosId { get => comentariosId; set => comentariosId = value; }
        [NotMapped]
        public List<UComentarioPictograma> ListaComentariosPictograma { get => listaComentariosPictograma; set => listaComentariosPictograma = value; }
    }
}
