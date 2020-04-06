using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios
{
    [Serializable] // Se declara que la clase USubscripcion se puede expresar en formato JSON
    [Table("subscripcion", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase USubscripcion
    public class USubscripcion
    {
        //variables
        private int id_subscripcion; //id para identificar la subscripcion
        private string subscripcion; //nombre de la subscripcion
        private string contenidoSubscripcion; //Contenido de la subscripcion
        private double valorSubscripcion;     //precio de la subscripcion   
        private string imagen_Subscripcion; //imagen de la subscripcion
        private int estado; //estado de la subscripcion
        private string token;

        [Key]
        [Column("id")]
        public int Id_subscripcion { get => id_subscripcion; set => id_subscripcion = value; }
        [Column("subscripcion")]
        public string Subscripcion { get => subscripcion; set => subscripcion = value; }
        [Column("contenido_subscripcion")]
        public string ContenidoSubscripcion { get => contenidoSubscripcion; set => contenidoSubscripcion = value; }
        [Column("valor_subscripcion")]
        public double ValorSubscripcion { get => valorSubscripcion; set => valorSubscripcion = value; }
        [Column("imagen_subscripcion")]
        public string Imagen_Subscripcion { get => imagen_Subscripcion; set => imagen_Subscripcion = value; }
       // [ForeignKey("fk_estado_subsc")]
        [Column("estado")]
        public int Estado { get => estado; set => estado = value; }
        [Column("token")]
        public string Token { get => token; set => token = value; }
    }
}
