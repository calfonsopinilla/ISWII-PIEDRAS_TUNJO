using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitarios
{
    [Table("punto_interes", Schema = "parque")]
    public class UPuntoInteres
    {
        private int id;
        private string descripcion;
        private string latitud;
        private string longitud;
        private string token;
        private DateTime last_modificacion;
        [Key]
        [Column("id")]
        public int Id { get =>id; set=> id=value; }
        [Column("descripcion")]
        public string Descripcion { get=>descripcion; set=> descripcion=value; }
        [Column("latitud")]
        public string Latitud { get=>latitud; set=>latitud=value; }
        [Column("longitud")]
        public string Longitud { get=>longitud; set=>longitud=value; }
        [Column("token")]
        public string Token { get=>token; set=>token=value; }
        [Column("last_modification")]
        public DateTime LastModification { get=>last_modificacion; set=>last_modificacion=value; }
    }
}
