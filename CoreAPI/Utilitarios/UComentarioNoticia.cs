using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable]
    [Table("comentario_noticia", Schema = "parque")]

    public class UComentarioNoticia {

        private long id;
        private DateTime fechaPublicacion;
        private string descripcion;
        private double calificacion;
        private bool reportado;
        private int usuarioId;
        private int noticia_id;
        private string nombreUsuario;
        private UUsuario usuario;

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
        [Column("noticia_id")]
        public int Noticia_id { get => noticia_id; set => noticia_id = value; }
        [Column("token")]
        public string Token { get; set; } = "";
        [Column("last_modification")]
        public DateTime LastModification { get; set; } = DateTime.Now;
        [NotMapped]
        public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }        
    }
}
