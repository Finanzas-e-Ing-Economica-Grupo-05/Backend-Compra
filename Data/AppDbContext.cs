using Microsoft.EntityFrameworkCore;
using backend_finanzas_grupo5.Models;

namespace backend_finanzas_grupo5.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Vehiculo> Vehiculos { get; set; }
    public DbSet<Simulacion> Simulaciones { get; set; }
    public DbSet<ResultadoSimulacion> ResultadosSimulacion { get; set; }
    public DbSet<CronogramaPago> CronogramaPagos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // USUARIO
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.IdUsuario);
            entity.Property(u => u.UsuDni).HasMaxLength(8).IsRequired();
            entity.Property(u => u.UsuCorreo).IsRequired();
            entity.HasIndex(u => u.UsuDni).IsUnique();
            entity.HasIndex(u => u.UsuCorreo).IsUnique();
        });

        // VEHICULO
        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(v => v.IdVehiculo);
        });

        // SIMULACION
        modelBuilder.Entity<Simulacion>(entity =>
        {
            entity.HasKey(s => s.IdSimulacion);

            entity.HasOne(s => s.Usuario)
                  .WithMany(u => u.Simulaciones)
                  .HasForeignKey(s => s.IdUsuario)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Vehiculo)
                  .WithMany(v => v.Simulaciones)
                  .HasForeignKey(s => s.IdVehiculo)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // RESULTADO_SIMULACION (1 a 1 con Simulacion)
        modelBuilder.Entity<ResultadoSimulacion>(entity =>
        {
            entity.HasKey(r => r.IdResultado);

            entity.HasOne(r => r.Simulacion)
                  .WithOne(s => s.ResultadoSimulacion)
                  .HasForeignKey<ResultadoSimulacion>(r => r.IdSimulacion)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // CRONOGRAMA_PAGO (1 a muchos con Simulacion)
        modelBuilder.Entity<CronogramaPago>(entity =>
        {
            entity.HasKey(c => c.IdCronograma);

            entity.HasOne(c => c.Simulacion)
                .WithMany(s => s.CronogramaPagos)
                .HasForeignKey(c => c.IdSimulacion)
                .OnDelete(DeleteBehavior.Cascade);

            // Evita filas duplicadas del mismo mes para la misma simulación
            entity.HasIndex(c => new { c.IdSimulacion, c.Mes }).IsUnique();
        });
    }
}