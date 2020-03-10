using System.Data.Entity;
using Utilitarios;

namespace Data {

    public class DAOMapeo : DbContext {

        // Variables
        private readonly string schema;

        public DAOMapeo() : base("name=PostgresConnection") { // Nombre de la cadena de conexión
            // Constructor
        }

        // Constructor estático
        static DAOMapeo() {
            Database.SetInitializer<DAOMapeo>(null);
        }

        protected override void OnModelCreating(DbModelBuilder builder) {

            builder.HasDefaultSchema(this.schema);
            base.OnModelCreating(builder);
        }

        public DbSet<UUsuario> Usuarios { get; set; }
        public DbSet<URol> Roles { get; set; }
        public DbSet<UNoticia> Noticias { get; set; }
        public DbSet<UInformacionParque> InformacionParque { get; set; }
        public DbSet<UComentarioNoticia> ComentariosNoticia { get; set; }
    }
}
