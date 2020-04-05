using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitarios
{
    [Table("reserva_ticket", Schema = "parque")]
    public class UReservaTicket
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("fecha_compra")]
        public DateTime FechaCompra { get; set; } = DateTime.Now;
        [Column("fecha_ingreso")]
        public DateTime FechaIngreso { get; set; }
        [Column("precio")]
        public double Precio { get; set; }
        [Column("cantidad")]
        public double Cantidad { get; set; }
        [Column("qr")]
        public string Qr { get; set; }
        [Column("token")]
        public string Token { get; set; } = "";
        [Column("last_modification")]
        public DateTime LastModification { get; set; } = DateTime.Now;
        [Column("estado_id")]
        public int EstadoId { get; set; } = 1;

        [Column("usuario_id")]
        public int UUsuarioId { get; set; }
        public UUsuario UUsuario { get; set; }
    }
}
