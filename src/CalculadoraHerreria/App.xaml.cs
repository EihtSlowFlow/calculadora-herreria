using System.Windows;
using CalculadoraHerreria.Data;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraHerreria;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Asegurar que la base de datos existe y está actualizada
        using var context = new AppDbContext();
        context.Database.EnsureCreated();
        DatabaseSeeder.Seed(context);
    }
}
