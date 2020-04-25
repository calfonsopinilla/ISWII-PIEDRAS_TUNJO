using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Utilitarios {

    [Table("push", Schema = "pushed")] 

    public class UPush {
        private long id;
        private string objetoPush;
        private DateTime fecha;
        private int estadoId;
        private string tokenId;

        [Key]
        [Column("id")]
        public long Id { get => id; set => id = value; }
        [Column("objeto_push")]
        public string ObjetoPush { get => objetoPush; set => objetoPush = value; }
        [Column("fecha")]
        public DateTime Fecha { get => fecha; set => fecha = value; }
        [Column("estado_id")]
        public int EstadoId { get => estadoId; set => estadoId = value; }
        [Column("token_id")]
        public string TokenId { get => tokenId; set => tokenId = value; }
    }
}
