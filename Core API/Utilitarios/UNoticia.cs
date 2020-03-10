using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("noticia", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UNoticia {

        // Variables
        private int id;
        private string titulo;
        private string descripcion;
        private DateTime fechaPublicacion;
        private List<string> imagenesUrl;
        private List<string> comentariosId;
        private double calificacion;
        private List<UComentarioNoticia> listaComentariosNoticia;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("titulo")]
        public string Titulo { get => titulo; set => titulo = value; }
        [Column("descripcion")]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        [Column("fecha_publicacion")]
        public DateTime FechaPublicacion { get => fechaPublicacion; set => fechaPublicacion = value; }
        [Column("imagenes_url")]
        public List<string> ImagenesUrl { get => imagenesUrl; set => imagenesUrl = value; }
        [Column("comentarios_id")]
        public List<string> ComentariosId { get => comentariosId; set => comentariosId = value; }
        [Column("calificacion")]
        public double Calificacion { get => calificacion; set => calificacion = value; }
        [NotMapped]
        public List<UComentarioNoticia> ListaNoticias { get => listaComentariosNoticia; set => listaComentariosNoticia = value; }
        
    }
}
