namespace CalculadoraHerreria.Core.Services;

/// <summary>
/// Representa una pieza solicitada para un plan de cortes con múltiples medidas.
/// </summary>
public class PiezaSolicitada
{
    /// <summary>Cantidad de piezas de esta medida por unidad de producto.</summary>
    public int Cantidad { get; set; }

    /// <summary>Largo de cada pieza en mm.</summary>
    public double LargoMm { get; set; }

    /// <summary>Descripción de uso o destino de la pieza.</summary>
    public string? DescripcionUso { get; set; }
}

/// <summary>
/// Pieza asignada a una barra específica en el plan de cortes.
/// </summary>
public class PiezaAsignada
{
    /// <summary>Largo de la pieza en mm.</summary>
    public double LargoMm { get; set; }

    /// <summary>Descripción de uso.</summary>
    public string? DescripcionUso { get; set; }
}

/// <summary>
/// Distribución de piezas en una barra individual.
/// </summary>
public class BarraDistribucion
{
    /// <summary>Índice de la barra (1-based).</summary>
    public int NumeroBarra { get; set; }

    /// <summary>Largo total de la barra en mm.</summary>
    public double LargoBarra { get; set; }

    /// <summary>Piezas asignadas a esta barra, en secuencia de corte.</summary>
    public List<PiezaAsignada> Piezas { get; set; } = new();

    /// <summary>Longitud utilizada en piezas (mm).</summary>
    public double LongitudUtilizada => Piezas.Sum(p => p.LargoMm);

    /// <summary>Material consumido por cortes en esta barra (mm).</summary>
    public double MaterialEnCortes { get; set; }

    /// <summary>Sobrante de esta barra (mm).</summary>
    public double Sobrante { get; set; }

    /// <summary>Indica si el sobrante es reutilizable según el mínimo configurado.</summary>
    public bool SobranteReutilizable { get; set; }
}

/// <summary>
/// Resultado completo de un plan de cortes con múltiples medidas.
/// </summary>
public class PlanCortesResult
{
    /// <summary>Cantidad total de barras necesarias.</summary>
    public int BarrasNecesarias { get; set; }

    /// <summary>Distribución detallada de piezas por barra.</summary>
    public List<BarraDistribucion> Barras { get; set; } = new();

    /// <summary>Pérdida total por herramienta (kerf) en mm.</summary>
    public double PerdidaTotalCortes { get; set; }

    /// <summary>Sobrante total de todas las barras en mm.</summary>
    public double SobranteTotal { get; set; }

    /// <summary>Resumen de piezas: cantidad total por descripción/largo.</summary>
    public List<ResumenPieza> ResumenPiezas { get; set; } = new();
}

/// <summary>
/// Resumen de una línea de piezas en el plan.
/// </summary>
public class ResumenPieza
{
    /// <summary>Largo de la pieza en mm.</summary>
    public double LargoMm { get; set; }

    /// <summary>Descripción de uso.</summary>
    public string? DescripcionUso { get; set; }

    /// <summary>Cantidad total de piezas de esta medida.</summary>
    public int CantidadTotal { get; set; }
}
