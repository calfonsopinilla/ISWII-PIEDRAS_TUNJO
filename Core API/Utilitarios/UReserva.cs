using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("reserva", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    class UReserva {

        // Variables
        private int id;
        private DateTime fechaCompra;
        private double precio;
        private int usuarioId;
        private int estadoId;
        private string estadoNombre;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("fecha_comrpa")]
        public DateTime FechaCompra { get => fechaCompra; set => fechaCompra = value; }
        [Column("precio")]
        public double Precio { get => precio; set => precio = value; }
        [ForeignKey("UsuarioId")]
        [Column("usuario_id")]
        public int UsuarioId { get => usuarioId; set => usuarioId = value; }
        [ForeignKey("EstadoId")]
        [Column("estado_id")]
        public int EstadoId { get => estadoId; set => estadoId = value; }
        [NotMapped]
        public string EstadoNombre { get => estadoNombre; set => estadoNombre = value; }
    }
}
