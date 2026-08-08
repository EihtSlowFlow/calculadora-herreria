using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CalculadoraHerreria.Data;
using CalculadoraHerreria.Views;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System;

namespace CalculadoraHerreria;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Inicializar base de datos y migraciones
            using var context = new AppDbContext();
            context.Database.Migrate();
            DatabaseSeeder.Seed(context, seedDemoData: false);

            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
