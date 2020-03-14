using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitarios
{

    [Serializable] 
    [Table("tokens", Schema = "security")] 

    public class UToken
    {

        private int id;
        private string token;
        private DateTime fechaGeneracion;
        private DateTime fechaVencimiento;
        private int aplicacionId;
        private int userId;

        [Key]
        [Column("id")]
        public int Id { get => id; set => id = value; }
        [Column("token")]
        public string Token { get => token; set => token = value; }
        [Column("fecha_generacion")]
        public DateTime FechaGeneracion { get => fechaGeneracion; set => fechaGeneracion = value; }
        [Column("fecha_vencimiento")]
        public DateTime FechaVencimiento { get => fechaVencimiento; set => fechaVencimiento = value; }
        [Column("aplicacion_id")]
        public int AplicacionId { get => aplicacionId; set => aplicacionId = value; }
        [Column("user_id")]
        public int UserId { get => userId; set => userId = value; }
    }
}
