using GastosPersonales.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GastosPersonales.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TipoGasto> TiposGasto { get; set; }
        public DbSet<FondoMonetario> FondosMonetarios { get; set; }
        public DbSet<Presupuesto> Presupuestos { get; set; }
        public DbSet<RegistroGastoEncabezado> RegistroGastoEncabezados { get; set; }
        public DbSet<RegistroGastoDetalle> RegistroGastoDetalles { get; set; }
        public DbSet<Deposito> Depositos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relaciones (igual que antes)
            builder.Entity<RegistroGastoDetalle>()
                .HasOne(d => d.RegistroGastoEncabezado)
                .WithMany(h => h.Detalles)
                .HasForeignKey(d => d.RegistroGastoEncabezadoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Presupuesto>()
                .HasOne(p => p.TipoGasto)
                .WithMany()
                .HasForeignKey(p => p.TipoGastoId);

            builder.Entity<RegistroGastoDetalle>()
                .HasOne(d => d.TipoGasto)
                .WithMany()
                .HasForeignKey(d => d.TipoGastoId);

            builder.Entity<RegistroGastoEncabezado>()
                .HasOne(r => r.FondoMonetario)
                .WithMany()
                .HasForeignKey(r => r.FondoMonetarioId);

            builder.Entity<Deposito>()
                .HasOne(d => d.FondoMonetario)
                .WithMany()
                .HasForeignKey(d => d.FondoMonetarioId);

            // Definir precisión y escala para propiedades decimal:
            builder.Entity<RegistroGastoDetalle>()
                .Property(d => d.Monto)
                .HasPrecision(18, 2);

            builder.Entity<Presupuesto>()
                .Property(p => p.Monto)
                .HasPrecision(18, 2);

            builder.Entity<Deposito>()
                .Property(d => d.Monto)
                .HasPrecision(18, 2);
        }
    }
}
