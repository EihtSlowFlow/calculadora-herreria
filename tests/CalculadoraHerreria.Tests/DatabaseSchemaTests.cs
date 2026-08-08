using CalculadoraHerreria.Data;
using CalculadoraHerreria.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;

namespace CalculadoraHerreria.Tests;

/// <summary>
/// Verifica que el esquema de base de datos se crea correctamente
/// y que se pueden realizar operaciones CRUD básicas.
/// </summary>
public class DatabaseSchemaTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly SqliteConnection _connection;

    public DatabaseSchemaTests()
    {
        // Usar base de datos SQLite en memoria para los tests con una conexión persistente
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.Migrate();
    }

    [Fact]
    public void PuedeCrearMaterialLineal()
    {
        var material = new Material
        {
            Nombre = "Tubo cuadrado 40x40x1.6",
            Familia = "Lineal",
            Forma = "Tubo cuadrado",
            MaterialBase = "Hierro",
            Dimensiones = "40x40",
            Espesor = 1.6,
            UnidadOriginal = "mm",
            LargoDisponible = 6000,
            Cantidad = 5,
            Condicion = "Completa"
        };

        _context.Materiales.Add(material);
        _context.SaveChanges();

        var saved = _context.Materiales.First(m => m.Nombre == "Tubo cuadrado 40x40x1.6");
        Assert.Equal("Lineal", saved.Familia);
        Assert.Equal(6000, saved.LargoDisponible);
        Assert.Equal(5, saved.Cantidad);
        Assert.Null(saved.AreaCalculada); // No es chapa
    }

    [Fact]
    public void PuedeCrearChapaConAreaCalculada()
    {
        var chapa = new Material
        {
            Nombre = "Chapa lisa 1000x2000",
            Familia = "Chapa",
            MaterialBase = "Hierro",
            Espesor = 1.6,
            UnidadOriginal = "mm",
            LargoDisponible = 2000,
            Ancho = 1000,
            Cantidad = 3,
            Condicion = "Completa"
        };

        _context.Materiales.Add(chapa);
        _context.SaveChanges();

        var saved = _context.Materiales.First(m => m.Familia == "Chapa");
        Assert.Equal(2_000_000, saved.AreaCalculada); // 2000 × 1000
    }

    [Fact]
    public void PuedeCrearMovimiento()
    {
        var material = new Material
        {
            Nombre = "Test",
            Familia = "Lineal",
            Forma = "Ángulo L",
            UnidadOriginal = "mm",
            Cantidad = 10,
            Condicion = "Completa"
        };
        _context.Materiales.Add(material);
        _context.SaveChanges();

        var movimiento = new Movimiento
        {
            MaterialId = material.Id,
            Tipo = "Consumo",
            CantidadAnterior = 10,
            CantidadPosterior = 8,
            SobranteMm = 2350,
            Fecha = DateTime.Now
        };

        _context.Movimientos.Add(movimiento);
        _context.SaveChanges();

        var saved = _context.Movimientos.First();
        Assert.Equal("Consumo", saved.Tipo);
        Assert.Equal(10, saved.CantidadAnterior);
        Assert.Equal(8, saved.CantidadPosterior);
    }

    [Fact]
    public void PuedeCrearPlantillaConPiezas()
    {
        var plantilla = new Plantilla
        {
            Nombre = "Mesa de prueba",
            Descripcion = "Plantilla de prueba",
            Piezas = new List<PlantillaPieza>
            {
                new() { Cantidad = 2, LargoMm = 600, DescripcionUso = "Lados largos" },
                new() { Cantidad = 2, LargoMm = 400, DescripcionUso = "Lados cortos" },
                new() { Cantidad = 4, LargoMm = 125, DescripcionUso = "Patas" }
            }
        };

        _context.Plantillas.Add(plantilla);
        _context.SaveChanges();

        var saved = _context.Plantillas
            .Include(p => p.Piezas)
            .First();

        Assert.Equal("Mesa de prueba", saved.Nombre);
        Assert.Equal(3, saved.Piezas.Count);
        Assert.Equal(8, saved.Piezas.Sum(p => p.Cantidad)); // 2+2+4
    }

    [Fact]
    public void RechazaMovimientoConMaterialInexistente()
    {
        var movimiento = new Movimiento
        {
            MaterialId = 9999, // ID inexistente
            Tipo = "Consumo",
            CantidadAnterior = 1,
            CantidadPosterior = 0,
            Fecha = DateTime.Now
        };

        _context.Movimientos.Add(movimiento);

        // SQLite debe lanzar una excepción de restricción de clave foránea
        Assert.Throws<DbUpdateException>(() => _context.SaveChanges());
    }

    [Fact]
    public void PuedeGuardarYLeerConfiguracion()
    {
        var config = new Configuracion
        {
            Clave = "SobranteMinimoMm",
            Valor = "100"
        };

        _context.Configuraciones.Add(config);
        _context.SaveChanges();

        var saved = _context.Configuraciones.Find("SobranteMinimoMm");
        Assert.NotNull(saved);
        Assert.Equal("100", saved!.Valor);
    }

    [Fact]
    public void SeederCargaDatosDeEjemplo()
    {
        DatabaseSeeder.Seed(_context, seedDemoData: true);

        Assert.Equal(6, _context.Materiales.Count());
        Assert.Equal(9, _context.Configuraciones.Count());
        Assert.Single(_context.Plantillas);
        Assert.Equal(3, _context.PlantillaPiezas.Count());
    }

    [Fact]
    public void SeederNoInsertaDuplicados()
    {
        DatabaseSeeder.Seed(_context, seedDemoData: true);
        DatabaseSeeder.Seed(_context, seedDemoData: true); // Segunda ejecución

        Assert.Equal(6, _context.Materiales.Count()); // No debe duplicar
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
