// Librerías
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Table("reserva_promociones", Schema = "parque")]
    public class UReservaPromocion {

        // Variables
        private int id;
        private DateTime fechaCompra;
        private double precio;
        private int usuarioId;
        private int estadoId;
        private string token;
        private DateTime lastModification;
        private int promocionId;

        // Métodos Set & Get
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("fecha_compra")]
        public DateTime FechaCompra { get => fechaCompra; set => fechaCompra = value; }
        [Column("precio")]
        public double Precio { get => precio; set => precio = value; }
        [Column("usuario_id")]
        public int UsuarioId { get => usuarioId; set => usuarioId = value; }
        [Column("estado_id")]
        public int EstadoId { get => estadoId; set => estadoId = value; }
        [Column("token")]
        public string Token { get => token; set => token = value; }
        [Column("last_modification")]
        public DateTime LastModification { get => lastModification; set => lastModification = value; }

        [Column("promocion_id")]
        public int UPromocionId { get => promocionId; set => promocionId = value; }
        public UPromocion UPromocion { get; set; }
    }
}
