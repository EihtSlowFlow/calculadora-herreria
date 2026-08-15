namespace CalculadoraHerreria.Core.Services;

/// <summary>
/// Motor de cálculo para plan de cortes con múltiples medidas.
/// Utiliza el algoritmo First Fit Decreasing (FFD) para minimizar
/// la cantidad de barras y maximizar sobrantes aprovechables.
/// </summary>
public static class PlanCortesService
{
    /// <summary>
    /// Calcula la distribución óptima de múltiples piezas sobre barras.
    /// </summary>
    /// <param name="piezas">Lista de piezas solicitadas (cantidad y largo por unidad de producto).</param>
    /// <param name="cantidadProductos">Cantidad de productos a fabricar (multiplicador).</param>
    /// <param name="largoBarra">Largo de cada barra en mm.</param>
    /// <param name="anchoCorte">Ancho de corte / kerf en mm. Default: 3 mm.</param>
    /// <param name="sobranteMinimoMm">Sobrante mínimo para considerar reutilizable (mm). Default: 100 mm.</param>
    /// <returns>Resultado con la distribución sugerida.</returns>
    public static PlanCortesResult Calcular(
        List<PiezaSolicitada> piezas,
        int cantidadProductos,
        double largoBarra,
        double anchoCorte = 3,
        double sobranteMinimoMm = 100)
    {
        // Validaciones
        if (piezas == null || piezas.Count == 0)
            throw new ArgumentException("Debe proporcionar al menos una pieza.", nameof(piezas));
        if (cantidadProductos <= 0)
            throw new ArgumentException("La cantidad de productos debe ser mayor a 0.", nameof(cantidadProductos));
        if (largoBarra <= 0)
            throw new ArgumentException("El largo de barra debe ser mayor a 0.", nameof(largoBarra));
        if (anchoCorte < 0)
            throw new ArgumentException("El ancho de corte no puede ser negativo.", nameof(anchoCorte));
        if (sobranteMinimoMm < 0)
            throw new ArgumentException("El sobrante mínimo no puede ser negativo.", nameof(sobranteMinimoMm));

        // Expandir piezas: multiplicar por cantidadProductos
        var piezasExpandidas = new List<PiezaAsignada>();
        var resumenPiezas = new List<ResumenPieza>();

        foreach (var pieza in piezas)
        {
            if (pieza.LargoMm <= 0)
                throw new ArgumentException($"El largo de pieza debe ser mayor a 0. Pieza: '{pieza.DescripcionUso}'.");
            if (pieza.Cantidad <= 0)
                throw new ArgumentException($"La cantidad de pieza debe ser mayor a 0. Pieza: '{pieza.DescripcionUso}'.");
            if (pieza.LargoMm > largoBarra)
                throw new ArgumentException(
                    $"La pieza '{pieza.DescripcionUso}' ({pieza.LargoMm} mm) excede el largo de barra ({largoBarra} mm).");

            int cantidadTotal = pieza.Cantidad * cantidadProductos;

            resumenPiezas.Add(new ResumenPieza
            {
                LargoMm = pieza.LargoMm,
                DescripcionUso = pieza.DescripcionUso,
                CantidadTotal = cantidadTotal
            });

            for (int i = 0; i < cantidadTotal; i++)
            {
                piezasExpandidas.Add(new PiezaAsignada
                {
                    LargoMm = pieza.LargoMm,
                    DescripcionUso = pieza.DescripcionUso
                });
            }
        }

        // FFD: ordenar piezas de mayor a menor
        piezasExpandidas.Sort((a, b) => b.LargoMm.CompareTo(a.LargoMm));

        // Distribuir con First Fit Decreasing
        var barras = new List<BarraDistribucion>();

        foreach (var pieza in piezasExpandidas)
        {
            bool asignada = false;

            // Buscar la primera barra donde quepa
            foreach (var barra in barras)
            {
                double espacioNecesario = pieza.LargoMm + anchoCorte;
                double espacioDisponible = barra.LargoBarra - barra.LongitudUtilizada - barra.MaterialEnCortes;

                if (espacioDisponible >= espacioNecesario)
                {
                    barra.Piezas.Add(pieza);
                    barra.MaterialEnCortes += anchoCorte;
                    asignada = true;
                    break;
                }
            }

            // Si no cabe en ninguna barra existente, crear una nueva
            if (!asignada)
            {
                var nuevaBarra = new BarraDistribucion
                {
                    NumeroBarra = barras.Count + 1,
                    LargoBarra = largoBarra,
                    MaterialEnCortes = anchoCorte
                };
                nuevaBarra.Piezas.Add(pieza);
                barras.Add(nuevaBarra);
            }
        }

        // Calcular sobrantes por barra
        double perdidaTotalCortes = 0;
        double sobranteTotal = 0;

        foreach (var barra in barras)
        {
            barra.Sobrante = barra.LargoBarra - barra.LongitudUtilizada - barra.MaterialEnCortes;
            barra.SobranteReutilizable = barra.Sobrante >= sobranteMinimoMm;
            perdidaTotalCortes += barra.MaterialEnCortes;
            sobranteTotal += barra.Sobrante;
        }

        return new PlanCortesResult
        {
            BarrasNecesarias = barras.Count,
            Barras = barras,
            PerdidaTotalCortes = perdidaTotalCortes,
            SobranteTotal = sobranteTotal,
            ResumenPiezas = resumenPiezas
        };
    }
}
