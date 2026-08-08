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

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
