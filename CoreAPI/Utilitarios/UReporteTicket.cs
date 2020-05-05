using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitarios
{
    public class UReporteTicket
    {
 
        public int Id { get; set; }
        public DateTime FechaCompra { get; set; } = DateTime.Now;
        public DateTime FechaIngreso { get; set; }
        public double Precio { get; set; }
        public double Cantidad { get; set; }
        public string Qr { get; set; }
        public string Token { get; set; } = "";
        public DateTime LastModification { get; set; } = DateTime.Now;
        public int EstadoId { get; set; } = 1;  
        public string tipoTicket { get; set; }  
        public string NumeroDocumento { get; set; }
        public int UUsuarioId { get; set; }
        public UUsuario UUsuario { get; set; }

    }
}
