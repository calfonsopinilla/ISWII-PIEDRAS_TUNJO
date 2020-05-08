using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Table("recuperar_clave", Schema = "parque")]
    public class URecuperarClave {

        // Variables
        private long id;
        private int userId;        
        private string token;
        private DateTime lastModification;

        // Métodos Set & Get
        [Key]
        [Column("id")]
        public long Id { get => id; set => id = value; }
        [Column("user_id")]
        public int UserId { get => userId; set => userId = value; }        
        [Column("token")]
        public string Token { get => token; set => token = value; }
        [Column("last_modification")]
        public DateTime LastModification { get => lastModification; set => lastModification = value; }        
    }
}
