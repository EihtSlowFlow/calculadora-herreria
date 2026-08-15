namespace CalculadoraHerreria.Core.Services;

/// <summary>
/// Resultado de un cálculo de desarrollo circular.
/// </summary>
public class DesarrolloCircularResult
{
    /// <summary>Circunferencia calculada (D × π) en mm.</summary>
    public double Circunferencia { get; set; }

    /// <summary>Margen agregado en mm.</summary>
    public double Margen { get; set; }

    /// <summary>Longitud final de cada pieza en mm (circunferencia + margen).</summary>
    public double LongitudPieza { get; set; }

    /// <summary>Longitud total para todas las piezas en mm.</summary>
    public double LongitudTotal { get; set; }

    /// <summary>Cantidad de piezas solicitadas.</summary>
    public int Cantidad { get; set; }
}
