using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Table("reserva_cabana", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UReservaCabana {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("fecha_reserva")]
        public DateTime FechaReserva { get; set; }
        [Column("valor_total")]
        public int ValorTotal { get; set; }
        [Column("last_modification")]
        public DateTime LastModification { get; set; } = DateTime.Now;
        [Column("usuario_id")]
        public int UUsuarioId { get; set; }

        [Column("cabana_id")]
        public int UCabanaId { get; set; }
        public UCabana UCabana { get; set; }
    }
}
