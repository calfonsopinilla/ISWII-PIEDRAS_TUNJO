using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("evento", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UEvento {

        // Variables
        private int id;
        private string nombre;
        private DateTime fechaPublicacion;
        private DateTime fecha;
        private string descripcion;
        private double calificacion;
        private string imagenesUrl;
        private List<string> comentariosId;
        private List<UComentarioEvento> listaComentariosEvento;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("nombre")]
        public string Nombre { get => nombre; set => nombre = value; }
        [Column("fecha_publicacion")]
        public DateTime FechaPublicacion { get => fechaPublicacion; set => fechaPublicacion = value; }
        [Column("fecha")]
        public DateTime Fecha { get => fecha; set => fecha = value; }
        [Column("descripcion")]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        [Column("calificacion")]
        public double Calificacion { get => calificacion; set => calificacion = value; }
        [Column("imagenes_url")]
        public string ImagenesUrl { get => imagenesUrl; set => imagenesUrl = value; }
        [Column("comentarios_id")]
        public List<string> ComentariosId { get => comentariosId; set => comentariosId = value; }
        [NotMapped]
        public List<UComentarioEvento> ListaComentariosEvento { get => listaComentariosEvento; set => listaComentariosEvento = value; }
    }
}
