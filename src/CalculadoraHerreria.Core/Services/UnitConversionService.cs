using System.Globalization;

namespace CalculadoraHerreria.Core.Services;

/// <summary>
/// Servicio de conversión de unidades de medida.
/// Todos los cálculos internos se realizan en milímetros.
/// Unidades soportadas: mm, cm, m, pulgadas.
/// </summary>
public static class UnitConversionService
{
    private static readonly Dictionary<string, double> FactoresAMm = new()
    {
        { "mm", 1.0 },
        { "cm", 10.0 },
        { "m", 1000.0 },
        { "pulgadas", 25.4 }
    };

    /// <summary>Convierte un valor desde cualquier unidad soportada a milímetros.</summary>
    public static double ConvertirAMm(double valor, string unidadOrigen)
    {
        ValidarUnidad(unidadOrigen);
        return valor * FactoresAMm[unidadOrigen];
    }

    /// <summary>Convierte un valor en milímetros a cualquier unidad soportada.</summary>
    public static double ConvertirDesdeMm(double valorMm, string unidadDestino)
    {
        ValidarUnidad(unidadDestino);
        return valorMm / FactoresAMm[unidadDestino];
    }

    /// <summary>Convierte un valor entre dos unidades cualesquiera.</summary>
    public static double Convertir(double valor, string unidadOrigen, string unidadDestino)
    {
        var mm = ConvertirAMm(valor, unidadOrigen);
        return ConvertirDesdeMm(mm, unidadDestino);
    }

    /// <summary>
    /// Formatea un valor numérico para mostrar en pantalla.
    /// Usa coma como separador decimal y punto como separador de miles.
    /// Resultados con hasta 2 decimales.
    /// </summary>
    public static string FormatearParaPantalla(double valor)
    {
        var cultura = new CultureInfo("es-AR");
        return valor.ToString("N2", cultura);
    }

    /// <summary>Verifica si una unidad es válida.</summary>
    public static bool EsUnidadValida(string unidad) =>
        FactoresAMm.ContainsKey(unidad);

    private static void ValidarUnidad(string unidad)
    {
        if (!EsUnidadValida(unidad))
        {
            throw new ArgumentException(
                $"Unidad '{unidad}' no es válida. Unidades soportadas: {string.Join(", ", FactoresAMm.Keys)}",
                nameof(unidad));
        }
    }
}
