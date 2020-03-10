using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("comentario", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UComentario {

        // Variables
        private long id;
        private DateTime fechaPublicacion;
        private string descripcion;
        private double calificacion;
        private bool reportado;
        private int usuarioId;

        [Key]
        [Column("id")]
        public long Id { get => id; set => id = value; }
        [Column("fecha_publicacion")]
        public DateTime FechaPublicacion { get => fechaPublicacion; set => fechaPublicacion = value; }
        [Column("descripcion")]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        [Column("calificacion")]
        public double Calificacion { get => calificacion; set => calificacion = value; }
        [Column("reportado")]
        public bool Reportado { get => reportado; set => reportado = value; }
        [ForeignKey("UsuarioId")]
        [Column("usuario_id")]
        public int UsuarioId { get => usuarioId; set => usuarioId = value; }
    }
}
