namespace CalculadoraHerreria.Core.Services;

/// <summary>
/// Motor de cálculo para desarrollo circular.
/// Fórmula: L = D × π + M
/// </summary>
public static class DesarrolloCircularService
{
    /// <summary>
    /// Calcula el desarrollo lineal de una pieza circular.
    /// </summary>
    /// <param name="diametro">Diámetro (D) en mm.</param>
    /// <param name="margen">Margen adicional para unión/soldadura/solapamiento (M) en mm. Default: 0.</param>
    /// <param name="cantidad">Cantidad de piezas a fabricar. Default: 1.</param>
    /// <returns>Resultado detallado del cálculo.</returns>
    public static DesarrolloCircularResult Calcular(
        double diametro,
        double margen = 0,
        int cantidad = 1)
    {
        if (diametro <= 0)
            throw new ArgumentException("El diámetro debe ser mayor a 0.", nameof(diametro));
        if (margen < 0)
            throw new ArgumentException("El margen no puede ser negativo.", nameof(margen));
        if (cantidad < 1)
            throw new ArgumentException("La cantidad debe ser al menos 1.", nameof(cantidad));

        double circunferencia = diametro * Math.PI;
        double longitudPieza = circunferencia + margen;
        double longitudTotal = longitudPieza * cantidad;

        return new DesarrolloCircularResult
        {
            Circunferencia = circunferencia,
            Margen = margen,
            LongitudPieza = longitudPieza,
            LongitudTotal = longitudTotal,
            Cantidad = cantidad
        };
    }
}
