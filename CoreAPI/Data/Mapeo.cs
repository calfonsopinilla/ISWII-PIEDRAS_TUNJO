using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data
{
    class Mapeo : DbContext
    {
        public Mapeo() : base("name=PostgresConnection")
        {
        }

        //private readonly string schema;

        public DbSet<UEvento> Eventos { get; set; }
        public DbSet<UUsuario> Usuarios { get; set; }
        public DbSet<UToken> token { get; set; }
        public DbSet<UInformacionParque> informacionParque{ get; set; }
        public DbSet<UTokenCorreo> TokenCorreo { get; set; }
        public DbSet<UNoticia> Noticias { get; set; }
        public DbSet<UPreguntas_frecuentes> preguntas_Frecuentes { get; set; }
        public DbSet<UPromocion> promocion { get; set; }
        public DbSet<UCabana> Cabana { get; set; }
        public DbSet<UPuntoInteres> PuntosInteres { get; set; }
        public DbSet<UPictograma> Pictograma { get; set; }
        //public DbSet<UComentarioPic> ComentarioPic { get; set; }
        public DbSet<UPuntuacion> Puntuacion { get; set; }
        public DbSet<UPQR> PQR { get; set; }
        public DbSet<UEstadoPQR> EstadoPqr { get; set; }        
        public DbSet<UTicket> Tickets { get; set; }
        public DbSet<UReservaTicket> ReservaTickets { get; set; }
        public DbSet<UComentarioNoticia> ComentariosNoticias { get; set; }
        public DbSet<UReservaCabana> ReservaCabanas { get; set; }
        public DbSet<UReservaPromocion> ReservaPromocion { get; set; }
        public DbSet<URecorrido> Recorridos { get; set; }
        public DbSet<URol> Roles { get; set; }

        /* Mapeo para comentarios */
        public DbSet<UComentarioCabana> ComentarioCabana { get; set; }
        public DbSet<UComentarioEvento> ComentarioEvento { get; set; }
        //public DbSet<UComentarioNoticia> ComentarioNoticia { get; set; }
        public DbSet<UComentarioPictograma> ComentarioPictograma { get; set; }

        public DbSet<UNotificacion> Notificacion { get; set; }

        public DbSet<UPush> Push { get; set; }

        public DbSet<URecuperarClave> RecuperarClave { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema(this.schema);
            base.OnModelCreating(modelBuilder);
        }
    }
}
