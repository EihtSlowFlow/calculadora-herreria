using Microsoft.EntityFrameworkCore;
using CalculadoraHerreria.Models;
using System.IO;

namespace CalculadoraHerreria.Data;

/// <summary>
/// Contexto de Entity Framework Core para la base de datos SQLite local.
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<Material> Materiales => Set<Material>();
    public DbSet<Movimiento> Movimientos => Set<Movimiento>();
    public DbSet<Plantilla> Plantillas => Set<Plantilla>();
    public DbSet<PlantillaPieza> PlantillaPiezas => Set<PlantillaPieza>();
    public DbSet<Configuracion> Configuraciones => Set<Configuracion>();

    /// <summary>
    /// Ruta de la base de datos SQLite.
    /// Se ubica en la carpeta de datos local del usuario (%LOCALAPPDATA% en Windows, ~/.local/share en Linux).
    /// </summary>
    private static string DbPath
    {
        get
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFolder = Path.Combine(folder, "CalculadoraHerreria");
            
            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }
            
            return Path.Combine(appFolder, "calculadora_herreria.db");
        }
    }

    /// <summary>
    /// Constructor sin parámetros para uso en producción.
    /// </summary>
    public AppDbContext() { }

    /// <summary>
    /// Constructor con opciones para inyección de dependencias y testing.
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Solo configurar SQLite si no se han proporcionado opciones externas (ej: InMemory para tests)
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración usa Clave como PK (string)
        modelBuilder.Entity<Configuracion>()
            .HasKey(c => c.Clave);

        // Índices útiles para búsqueda y filtrado
        modelBuilder.Entity<Material>()
            .HasIndex(m => m.Familia);

        modelBuilder.Entity<Material>()
            .HasIndex(m => m.Forma);

        modelBuilder.Entity<Material>()
            .HasIndex(m => m.Condicion);

        modelBuilder.Entity<Movimiento>()
            .HasIndex(m => m.MaterialId);

        modelBuilder.Entity<Movimiento>()
            .HasIndex(m => m.Fecha);

        modelBuilder.Entity<PlantillaPieza>()
            .HasIndex(p => p.PlantillaId);

        // Evitar que al eliminar un material se eliminen sus movimientos en cascada
        modelBuilder.Entity<Movimiento>()
            .HasOne(m => m.Material)
            .WithMany()
            .HasForeignKey(m => m.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
