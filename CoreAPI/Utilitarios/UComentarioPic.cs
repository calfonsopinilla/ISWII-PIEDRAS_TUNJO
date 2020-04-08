using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios
{

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("comentario_pictograma", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UComentarioPic
    {

        // Variables
        private int pictogramaId;

        private DateTime fecha_publicacion;
        private string descripcionpic;
        private double calificacionpic;
        private Boolean reportadopic;
        private int usuario_id;


        //[ForeignKey("PictogramaId")]
        [Key]
        [Column("pictograma_id")]
        public int PictogramaId { get => pictogramaId; set => pictogramaId = value; }
        [Column("fecha_publicacion")]
        public DateTime Fecha_publicacion { get => fecha_publicacion; set => fecha_publicacion = value; }
        [Column("descripcion")]
        public string Descripcionpic { get => descripcionpic; set => descripcionpic = value; }
        [Column("calificacion")]
        public double Calificacionpic { get => calificacionpic; set => calificacionpic = value; }
        [Column("reportado")]
        public bool Reportadopic { get => reportadopic; set => reportadopic = value; }
        [Column("usuario_id")]
        public int Usuario_id { get => usuario_id; set => usuario_id = value; }
    }
}