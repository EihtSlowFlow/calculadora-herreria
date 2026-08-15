namespace CalculadoraHerreria.Core.Services;

/// <summary>
/// Motor de cálculo para corte simple de piezas iguales a partir de barras.
/// Fórmula base: n(P + K) ≤ B
/// </summary>
public static class CorteSimpleService
{
    /// <summary>
    /// Calcula el corte simple de piezas iguales.
    /// </summary>
    /// <param name="largoBarra">Largo de la barra (B) en mm.</param>
    /// <param name="largoPieza">Largo de cada pieza (P) en mm.</param>
    /// <param name="anchoCorte">Ancho de corte / kerf (K) en mm. Default: 3 mm.</param>
    /// <param name="cantidadDeseada">Cantidad total de piezas deseadas (opcional).</param>
    /// <param name="margen">Margen / tolerancia adicional por pieza en mm. Default: 0.</param>
    /// <returns>Resultado detallado del cálculo.</returns>
    public static CorteSimpleResult Calcular(
        double largoBarra,
        double largoPieza,
        double anchoCorte = 3,
        int? cantidadDeseada = null,
        double margen = 0)
    {
        // Validaciones
        if (largoBarra <= 0)
            throw new ArgumentException("El largo de barra debe ser mayor a 0.", nameof(largoBarra));
        if (largoPieza <= 0)
            throw new ArgumentException("El largo de pieza debe ser mayor a 0.", nameof(largoPieza));
        if (anchoCorte < 0)
            throw new ArgumentException("El ancho de corte no puede ser negativo.", nameof(anchoCorte));
        if (margen < 0)
            throw new ArgumentException("El margen no puede ser negativo.", nameof(margen));

        double piezaEfectiva = largoPieza + margen;
        double paso = piezaEfectiva + anchoCorte;

        // n(P + K) ≤ B → n = floor(B / (P + K))
        int piezasPorBarra = paso > 0 ? (int)Math.Floor(largoBarra / paso) : 0;

        // Si la pieza es más grande que la barra
        if (piezasPorBarra == 0)
        {
            return new CorteSimpleResult
            {
                PiezasPorBarra = 0,
                CortesPorBarra = 0,
                LongitudUtil = 0,
                MaterialEnCortes = 0,
                SobrantePorBarra = largoBarra,
                BarrasNecesarias = cantidadDeseada.HasValue ? (int?)null : null,
                SobranteTotal = null,
                TotalPiezas = cantidadDeseada.HasValue ? 0 : null
            };
        }

        int cortesPorBarra = piezasPorBarra;
        double longitudUtil = piezasPorBarra * piezaEfectiva;
        double materialEnCortes = piezasPorBarra * anchoCorte;
        double sobrantePorBarra = largoBarra - piezasPorBarra * paso;

        var result = new CorteSimpleResult
        {
            PiezasPorBarra = piezasPorBarra,
            CortesPorBarra = cortesPorBarra,
            LongitudUtil = longitudUtil,
            MaterialEnCortes = materialEnCortes,
            SobrantePorBarra = sobrantePorBarra
        };

        if (cantidadDeseada.HasValue)
        {
            int cantidad = cantidadDeseada.Value;
            int barrasNecesarias = (int)Math.Ceiling((double)cantidad / piezasPorBarra);

            // Calcular sobrante total
            int barrasCompletas = cantidad / piezasPorBarra;
            int piezasRestantes = cantidad % piezasPorBarra;

            double sobranteTotal = barrasCompletas * sobrantePorBarra;

            if (piezasRestantes > 0)
            {
                // La última barra tiene menos piezas
                double sobranteUltimaBarra = largoBarra - piezasRestantes * paso;
                sobranteTotal += sobranteUltimaBarra;
            }

            result.BarrasNecesarias = barrasNecesarias;
            result.TotalPiezas = cantidad;
            result.SobranteTotal = sobranteTotal;
        }

        return result;
    }
}
