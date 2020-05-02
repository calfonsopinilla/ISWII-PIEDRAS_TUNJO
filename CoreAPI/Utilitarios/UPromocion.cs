using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios
{
    //[Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("promocion", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UPromocion
    {
        // Variables
        private int id;
        private string nombre;
        private string descripcion;
        private string estado; // 1 significa que la promoción esta activa - 2 significa que la promoción no esta activa        
        private DateTime fechaInicio;
        private DateTime fechaFin;
        private double porcentajeDescuento;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("nombre")]
        public string Nombre { get => nombre; set => nombre = value; }
        [Column("descripcion")]
        public string Descripcion { get => descripcion; set => descripcion = value; }
        //[Column("precio")]
        //public double Precio { get => precio; set => precio = value; }
        [Column("fecha_inicio")]
        public DateTime FechaInicio { get => fechaInicio; set => fechaInicio = value; }
        [Column("fecha_fin")]
        public DateTime FechaFin { get => fechaFin; set => fechaFin = value; }
        [Column("porcentaje_descuento")]
        public double PorcentajeDescuento { get => porcentajeDescuento; set => porcentajeDescuento = value; }
        [Column("estado")]
        public string Estado { get => estado; set => estado = value; }
        [Column("token")]
        public string Token { get; set; }
        [Column("last_modification")]
        public DateTime LastModification { get; set; } = DateTime.Now;
    }
}
