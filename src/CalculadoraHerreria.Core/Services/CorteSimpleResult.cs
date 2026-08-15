namespace CalculadoraHerreria.Core.Services;

/// <summary>
/// Resultado de un cálculo de corte simple de piezas iguales.
/// </summary>
public class CorteSimpleResult
{
    /// <summary>Piezas obtenibles por barra.</summary>
    public int PiezasPorBarra { get; set; }

    /// <summary>Barras necesarias para la cantidad deseada (null si no se especificó cantidad).</summary>
    public int? BarrasNecesarias { get; set; }

    /// <summary>Cantidad de cortes por barra.</summary>
    public int CortesPorBarra { get; set; }

    /// <summary>Longitud útil convertida en piezas (mm).</summary>
    public double LongitudUtil { get; set; }

    /// <summary>Material consumido por cortes por barra (mm).</summary>
    public double MaterialEnCortes { get; set; }

    /// <summary>Sobrante por barra (mm).</summary>
    public double SobrantePorBarra { get; set; }

    /// <summary>Sobrante total considerando todas las barras (mm). Null si no se indicó cantidad.</summary>
    public double? SobranteTotal { get; set; }

    /// <summary>Total de piezas obtenidas. Null si no se indicó cantidad.</summary>
    public int? TotalPiezas { get; set; }
}
