using System.Threading.Tasks;
using CalculadoraHerreria.Core.Services;
using CalculadoraHerreria.Data;
using CalculadoraHerreria.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CalculadoraHerreria.Tests.Services;

public class SobranteMinimoServiceTests : System.IDisposable
{
    private readonly AppDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly SobranteMinimoService _service;

    public SobranteMinimoServiceTests()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.Migrate();
        DatabaseSeeder.Seed(_context, seedDemoData: false);

        _service = new SobranteMinimoService(_context);
    }

    [Fact]
    public async Task ObtenerSobrante_ConSeeder_Retorna100()
    {
        // El seeder inserta SobranteMinimoMm = "100"
        var minimo = await _service.ObtenerSobranteMinimoAsync();
        Assert.Equal(100, minimo);
    }

    [Fact]
    public async Task ActualizarYObtener_NuevoValor()
    {
        await _service.ActualizarSobranteMinimoAsync(250);
        var minimo = await _service.ObtenerSobranteMinimoAsync();
        Assert.Equal(250, minimo);
    }

    [Fact]
    public async Task EsSobranteReutilizable_Sobre100_EsTrue()
    {
        var resultado = await _service.EsSobranteReutilizableAsync(150);
        Assert.True(resultado);
    }

    [Fact]
    public async Task EsSobranteReutilizable_Bajo100_EsFalse()
    {
        var resultado = await _service.EsSobranteReutilizableAsync(50);
        Assert.False(resultado);
    }

    [Fact]
    public async Task EsSobranteReutilizable_Exacto100_EsTrue()
    {
        var resultado = await _service.EsSobranteReutilizableAsync(100);
        Assert.True(resultado);
    }

    [Fact]
    public async Task ActualizarConValorNegativo_LanzaExcepcion()
    {
        await Assert.ThrowsAsync<System.ArgumentException>(() =>
            _service.ActualizarSobranteMinimoAsync(-10));
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
