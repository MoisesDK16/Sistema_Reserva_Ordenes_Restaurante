using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurante_Moises_Loor.Models;

namespace Sistema_ReservaOrdenes_Restaurante.Data
{
    public class RestauranteContext : IdentityDbContext
    {
        public RestauranteContext(DbContextOptions<RestauranteContext> options): base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<Detalle> Detalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>().ToTable("usuario");
            modelBuilder.Entity<Menu>().ToTable("menu");
            modelBuilder.Entity<Orden>().ToTable("orden");
            modelBuilder.Entity<Detalle>().ToTable("detalle");
        }
    }
}
