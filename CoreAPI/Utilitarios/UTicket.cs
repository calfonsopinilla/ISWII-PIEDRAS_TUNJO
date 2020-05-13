using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitarios
{
    [Table("ticket", Schema = "parque")]

    public class UTicket
    {
        private int id;
        private string nombre;
        private double precio;
        private string descripcion;
        private int estado;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("nombre")]
        public string Nombre { get => nombre; set => nombre = value; }
        [Column("precio")]
        public double Precio { get => precio; set => precio = value; }
        [Column("descripcion")]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        [Column("token")]
        public string Token { get; set; }
        [Column("last_modification")]
        public DateTime LastModificacion { get; set; }
        [Column("estado")]
        public int Estado { get => estado; set => estado = value; }
    }
}
