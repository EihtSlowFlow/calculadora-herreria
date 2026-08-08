using CalculadoraHerreria.Models;

namespace CalculadoraHerreria.Data;

/// <summary>
/// Inserta datos iniciales de ejemplo en la base de datos si está vacía.
/// </summary>
public static class DatabaseSeeder
{
    public static void Seed(AppDbContext context, bool seedDemoData = false)
    {
        // --- Configuraciones iniciales (Idempotente) ---
        var configuracionesPorDefecto = new Dictionary<string, string>
        {
            { "UnidadPreferida", "mm" },
            { "AnchoCorteDefault", "3" },
            { "SobranteMinimoMm", "100" },
            { "LargoBarraDefaultMm", "6000" },
            { "VentanaCalculadora_X", "" },
            { "VentanaCalculadora_Y", "" },
            { "VentanaCalculadora_Ancho", "400" },
            { "VentanaCalculadora_Alto", "500" },
            { "VentanaCalculadora_SiempreVisible", "false" }
        };

        foreach (var kvp in configuracionesPorDefecto)
        {
            if (context.Configuraciones.Find(kvp.Key) == null)
            {
                context.Configuraciones.Add(new Configuracion { Clave = kvp.Key, Valor = kvp.Value });
            }
        }
        
        context.SaveChanges();

        // Si no se solicitaron datos de demostración, salir aquí.
        if (!seedDemoData)
            return;

        // Solo sembrar demo data si no hay materiales
        if (context.Materiales.Any())
            return;

        // --- Materiales lineales de ejemplo ---
        var materiales = new List<Material>
        {
            new()
            {
                Nombre = "Ángulo L 25x25x3",
                Familia = "Lineal",
                Forma = "Ángulo L",
                MaterialBase = "Hierro",
                Dimensiones = "25x25",
                Espesor = 3.18,
                UnidadOriginal = "mm",
                LargoDisponible = 6000,
                Cantidad = 10,
                Condicion = "Completa",
                Observaciones = "Material de ejemplo"
            },
            new()
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
            },
            new()
            {
                Nombre = "Planchuela 25x3",
                Familia = "Lineal",
                Forma = "Planchuela",
                MaterialBase = "Hierro",
                Dimensiones = "25x3",
                Espesor = 3.18,
                UnidadOriginal = "mm",
                LargoDisponible = 6000,
                Cantidad = 8,
                Condicion = "Completa"
            },
            new()
            {
                Nombre = "Tubo redondo 25x1.2",
                Familia = "Lineal",
                Forma = "Tubo redondo",
                MaterialBase = "Hierro",
                Dimensiones = "25.4",
                Espesor = 1.2,
                UnidadOriginal = "mm",
                LargoDisponible = 6000,
                Cantidad = 12,
                Condicion = "Completa"
            },
            new()
            {
                Nombre = "Sobrante tubo cuadrado 40x40",
                Familia = "Lineal",
                Forma = "Tubo cuadrado",
                MaterialBase = "Hierro",
                Dimensiones = "40x40",
                Espesor = 1.6,
                UnidadOriginal = "mm",
                LargoDisponible = 2350,
                Cantidad = 2,
                Condicion = "Sobrante",
                Observaciones = "Sobrante de trabajo anterior"
            },
            // --- Chapa de ejemplo ---
            new()
            {
                Nombre = "Chapa lisa 1000x2000 e=1.6",
                Familia = "Chapa",
                Forma = null,
                MaterialBase = "Hierro",
                Dimensiones = "1000x2000",
                Espesor = 1.6,
                UnidadOriginal = "mm",
                LargoDisponible = 2000,
                Ancho = 1000,
                Cantidad = 3,
                Condicion = "Completa"
            }
        };

        context.Materiales.AddRange(materiales);

        // --- Plantilla de ejemplo (del plan: mesa básica) ---
        var plantilla = new Plantilla
        {
            Nombre = "Mesa básica (ejemplo)",
            Descripcion = "Estructura de mesa con 4 patas y marco rectangular",
            Piezas = new List<PlantillaPieza>
            {
                new() { Cantidad = 2, LargoMm = 600, DescripcionUso = "Lados largos" },
                new() { Cantidad = 2, LargoMm = 400, DescripcionUso = "Lados cortos" },
                new() { Cantidad = 4, LargoMm = 125, DescripcionUso = "Patas" }
            }
        };

        context.Plantillas.Add(plantilla);

        context.SaveChanges();
    }
}
