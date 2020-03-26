using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("pqr", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UPQR {

        // Variables
        private int id;
        private DateTime fechaPublicacion;
        private string pregunta;
        private string respuesta;
        private int usuarioId;
        private int estadoId;
        private bool estado;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("fecha_publicacion")]
        public DateTime FechaPublicacion { get => fechaPublicacion; set => fechaPublicacion = value; }
        [Column("pregunta")]
        public string Pregunta { get => pregunta; set => pregunta = value; }
        [Column("respuesta")]
        public string Respuesta { get => respuesta; set => respuesta = value; }
        //[ForeignKey("UsuarioId")]
        [Column("usuario_id")]
        public int UsuarioId { get => usuarioId; set => usuarioId = value; }
        //[ForeignKey("EstadoId")]
        [Column("estado_id")]
        public int EstadoId { get => estadoId; set => estadoId = value; }
        [Column("token")]
        public string Token { get; set; }
        [Column("last_nodification")]
        public string LastModificacion { get; set; }
        [Column("estado")]
        public bool Estado { get => estado; set => estado = value; }



    }
}
