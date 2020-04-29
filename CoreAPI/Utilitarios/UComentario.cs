using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("comentarios", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UComentario {

        // Variables
        private long id;
        private DateTime fechaPublicacion;
        private string descripcion;
        private double calificacion;
        private bool reportado;
        private int usuarioId;
        private UUsuario usuario;
        private DateTime lastModification;
        private string token;

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
        [Column("usuario_id")]
        public int UsuarioId { get => usuarioId; set => usuarioId = value; }
        public UUsuario Usuario { get => usuario; set => usuario = value; }
        [Column("last_modification")]
        public DateTime LastModification { get => lastModification; set => lastModification = value; }
        [Column("token")]
        public string Token { get => token; set => token = value; }        
    }
}
