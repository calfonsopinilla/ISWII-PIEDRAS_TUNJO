using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("cabana", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UCabana {

        // Variables
        private int id;
        private string nombre;
        private int capacidad;
        private double precio;
        private double calificacion;
        private List<string> imagenesUrl;
        private List<string> comentariosId;
        private List<UComentarioCabana> listaComentariosCabana;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("nombre")]
        public string Nombre { get => nombre; set => nombre = value; }
        [Column("capacidad")]
        public int Capacidad { get => capacidad; set => capacidad = value; }
        [Column("precio")]
        public double Precio { get => precio; set => precio = value; }
        [Column("calificacion")]
        public double Calificacion { get => calificacion; set => calificacion = value; }
        [Column("imagenes_url")]
        public List<string> ImagenesUrl { get => imagenesUrl; set => imagenesUrl = value; }
        [Column("comentarios_id")]
        public List<string> ComentariosId { get => comentariosId; set => comentariosId = value; }
        [NotMapped]
        public List<UComentarioCabana> ListaComentariosCabana { get => listaComentariosCabana; set => listaComentariosCabana = value; }
    }
}
