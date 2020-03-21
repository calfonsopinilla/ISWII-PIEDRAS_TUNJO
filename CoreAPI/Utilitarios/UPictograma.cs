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
        private double calificacion;
        private string descripcion;
        private string imagenes_url;
       // private List<string> comentariosId;
        private List<UComentarioPictograma> listaComentariosPictograma;
        private int estado;
        private int id_parque;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("nombre")]
        public string Nombre { get => nombre; set => nombre = value; }
        [Column("descripcion")]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        [Column("imagenes_url")]
        public string Imagenes_url { get => imagenes_url; set => imagenes_url = value; }
        [Column("calificacion")]
        public double Calificacion { get => calificacion; set => calificacion = value; }
        [Column("estado")]
        public int Estado { get => estado; set => estado = value; }
        [Column("id_parque")]
        public int Id_parque { get => id_parque; set => id_parque = value; }
        //  [Column("comentarios_id")]
        // public List<string> ComentariosId { get => comentariosId; set => comentariosId = value; }

        [NotMapped]
        public List<UComentarioPictograma> ListaComentariosPictograma { get => listaComentariosPictograma; set => listaComentariosPictograma = value; }
        
    }
}
