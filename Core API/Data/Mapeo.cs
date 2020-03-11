﻿using System;
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
        private readonly string schema;

        public DbSet<UEvento> Eventos { get; set; }
        public DbSet<UUsuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(this.schema);
            base.OnModelCreating(modelBuilder);
        }
    }
}
