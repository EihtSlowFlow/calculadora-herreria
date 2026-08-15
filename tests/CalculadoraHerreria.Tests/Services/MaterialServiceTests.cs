using System.Linq;
using System.Threading.Tasks;
using CalculadoraHerreria.Core.Services;
using CalculadoraHerreria.Data;
using CalculadoraHerreria.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CalculadoraHerreria.Tests.Services;

public class MaterialServiceTests : System.IDisposable
{
    private readonly AppDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly MaterialService _service;

    public MaterialServiceTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.Migrate();

        _service = new MaterialService(_context);
    }

    [Fact]
    public async Task ObtenerLinealesAsync_SinFiltro_RetornaSoloLineales()
    {
        DatabaseSeeder.Seed(_context, seedDemoData: true);
        
        var lineales = await _service.ObtenerLinealesAsync();
        
        Assert.NotEmpty(lineales);
        Assert.All(lineales, m => Assert.Equal("Lineal", m.Familia));
    }

    [Fact]
    public async Task ObtenerLinealesAsync_ConFiltroForma_RetornaSoloFormaEspecifica()
    {
        DatabaseSeeder.Seed(_context, seedDemoData: true);
        
        var tuboCuadrado = await _service.ObtenerLinealesAsync(forma: "Tubo cuadrado");
        
        Assert.Equal(2, tuboCuadrado.Count); // El seeder tiene uno completo y un sobrante
        Assert.All(tuboCuadrado, m => Assert.Equal("Tubo cuadrado", m.Forma));
    }

    [Fact]
    public async Task ObtenerLinealesAsync_ConFiltroCondicion_RetornaSoloCondicionEspecifica()
    {
        DatabaseSeeder.Seed(_context, seedDemoData: true);
        
        var sobrantes = await _service.ObtenerLinealesAsync(condicion: "Sobrante");
        
        Assert.Single(sobrantes);
        Assert.Equal("Sobrante tubo cuadrado 40x40", sobrantes.First().Nombre);
    }

    [Fact]
    public async Task ObtenerChapasAsync_RetornaSoloChapas()
    {
        DatabaseSeeder.Seed(_context, seedDemoData: true);
        
        var chapas = await _service.ObtenerChapasAsync();
        
        Assert.Single(chapas);
        Assert.Equal("Chapa", chapas.First().Familia);
        // Validar área de la chapa
        Assert.Equal(2000000, chapas.First().AreaCalculada);
    }

    [Fact]
    public async Task AgregarMaterial_FamiliaInvalida_LanzaExcepcion()
    {
        var material = new Material
        {
            Nombre = "Test",
            Familia = "foo",
            Condicion = "Completa",
            UnidadOriginal = "mm"
        };

        await Assert.ThrowsAsync<System.ArgumentException>(() =>
            _service.AgregarMaterialAsync(material));
    }

    [Fact]
    public async Task AgregarMaterial_CondicionInvalida_LanzaExcepcion()
    {
        var material = new Material
        {
            Nombre = "Test",
            Familia = "Lineal",
            Forma = "Planchuela",
            Condicion = "Rota",
            UnidadOriginal = "mm",
            LargoDisponible = 6000
        };

        await Assert.ThrowsAsync<System.ArgumentException>(() =>
            _service.AgregarMaterialAsync(material));
    }

    [Fact]
    public async Task AgregarMaterial_UnidadInvalida_LanzaExcepcion()
    {
        var material = new Material
        {
            Nombre = "Test",
            Familia = "Lineal",
            Forma = "Planchuela",
            Condicion = "Completa",
            UnidadOriginal = "pies",
            LargoDisponible = 6000
        };

        await Assert.ThrowsAsync<System.ArgumentException>(() =>
            _service.AgregarMaterialAsync(material));
    }

    [Fact]
    public async Task AgregarMaterial_CantidadNegativa_LanzaExcepcion()
    {
        var material = new Material
        {
            Nombre = "Test",
            Familia = "Lineal",
            Forma = "Planchuela",
            Condicion = "Completa",
            UnidadOriginal = "mm",
            LargoDisponible = 6000,
            Cantidad = -1
        };

        await Assert.ThrowsAsync<System.ArgumentException>(() =>
            _service.AgregarMaterialAsync(material));
    }

    [Fact]
    public async Task AgregarMaterial_FormaInvalidaParaLineal_LanzaExcepcion()
    {
        var material = new Material
        {
            Nombre = "Test",
            Familia = "Lineal",
            Forma = "Inexistente",
            Condicion = "Completa",
            UnidadOriginal = "mm",
            LargoDisponible = 6000
        };

        await Assert.ThrowsAsync<System.ArgumentException>(() =>
            _service.AgregarMaterialAsync(material));
    }

    [Fact]
    public async Task AgregarMaterial_ChapasSinAncho_LanzaExcepcion()
    {
        var material = new Material
        {
            Nombre = "Chapa test",
            Familia = "Chapa",
            Condicion = "Completa",
            UnidadOriginal = "mm",
            LargoDisponible = 2000,
            Ancho = null
        };

        await Assert.ThrowsAsync<System.ArgumentException>(() =>
            _service.AgregarMaterialAsync(material));
    }

    [Fact]
    public async Task AgregarMaterial_MaterialValido_SePersiste()
    {
        var material = new Material
        {
            Nombre = "Ángulo test",
            Familia = "Lineal",
            Forma = "Ángulo L",
            Condicion = "Completa",
            UnidadOriginal = "mm",
            LargoDisponible = 6000,
            Cantidad = 5
        };

        var result = await _service.AgregarMaterialAsync(material);
        Assert.True(result.Id > 0);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
