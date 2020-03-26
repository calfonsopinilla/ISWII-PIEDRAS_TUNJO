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
        public DbSet<USubscripcion> infoSubscripcion { get; set; }
        public DbSet<UPreguntas_frecuentes> preguntas_Frecuentes { get; set; }
        public DbSet<UPromocion> promocion { get; set; }
        public DbSet<UCabana> Cabana { get; set; }
        public DbSet<UPuntoInteres> PuntosInteres { get; set; }
        public DbSet<UPictograma> Pictograma { get; set; }
<<<<<<< HEAD
        public DbSet<UPuntuacion> Puntuacion { get; set; }
        public DbSet<UPQR> pqr { get; set; }
        public DbSet<UPQR> EstadoPqr { get; set; }        
=======

        public DbSet<UPQR> pqr { get; set; }

        public DbSet<UEstadoPQR> EstadoPqr { get; set; }
>>>>>>> 9d9b6c5cd282910e3ba7d92c366d37dd060f7552

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema(this.schema);
            base.OnModelCreating(modelBuilder);
        }
    }
}
