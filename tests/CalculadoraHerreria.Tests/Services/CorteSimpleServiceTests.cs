using CalculadoraHerreria.Core.Services;
using Xunit;

namespace CalculadoraHerreria.Tests.Services;

public class CorteSimpleServiceTests
{
    [Fact]
    public void CasoDocumentado_Barra6000_Pieza370_Kerf3()
    {
        // Caso de prueba del Issue #6:
        // Barra 6000 mm, pieza 370 mm, kerf 3 mm → 16 piezas, 48 mm en cortes, 32 mm sobrante.
        var result = CorteSimpleService.Calcular(
            largoBarra: 6000,
            largoPieza: 370,
            anchoCorte: 3);

        Assert.Equal(16, result.PiezasPorBarra);
        Assert.Equal(16, result.CortesPorBarra);
        Assert.Equal(5920, result.LongitudUtil);       // 16 * 370
        Assert.Equal(48, result.MaterialEnCortes);       // 16 * 3
        Assert.Equal(32, result.SobrantePorBarra);       // 6000 - 16*373
        Assert.Null(result.BarrasNecesarias);
        Assert.Null(result.SobranteTotal);
        Assert.Null(result.TotalPiezas);
    }

    [Fact]
    public void Barra6000_Pieza1000_Kerf3()
    {
        // floor(6000/1003) = 5 piezas
        var result = CorteSimpleService.Calcular(6000, 1000, 3);

        Assert.Equal(5, result.PiezasPorBarra);
        Assert.Equal(5000, result.LongitudUtil);         // 5 * 1000
        Assert.Equal(15, result.MaterialEnCortes);        // 5 * 3
        Assert.Equal(985, result.SobrantePorBarra);       // 6000 - 5*1003
    }

    [Fact]
    public void Barra6000_Pieza2990_Kerf3()
    {
        // floor(6000/2993) = 2 piezas
        var result = CorteSimpleService.Calcular(6000, 2990, 3);

        Assert.Equal(2, result.PiezasPorBarra);
        Assert.Equal(5980, result.LongitudUtil);          // 2 * 2990
        Assert.Equal(6, result.MaterialEnCortes);          // 2 * 3
        Assert.Equal(14, result.SobrantePorBarra);         // 6000 - 2*2993
    }

    [Fact]
    public void PiezaMayorQueBarra_RetornaCeroPiezas()
    {
        var result = CorteSimpleService.Calcular(1000, 1500, 3);

        Assert.Equal(0, result.PiezasPorBarra);
        Assert.Equal(0, result.CortesPorBarra);
        Assert.Equal(0, result.LongitudUtil);
        Assert.Equal(0, result.MaterialEnCortes);
        Assert.Equal(1000, result.SobrantePorBarra);
    }

    [Fact]
    public void ConCantidadDeseada_CalculaBarrasNecesarias()
    {
        // 16 piezas/barra, se necesitan 40 piezas → ceil(40/16) = 3 barras
        var result = CorteSimpleService.Calcular(6000, 370, 3, cantidadDeseada: 40);

        Assert.Equal(16, result.PiezasPorBarra);
        Assert.Equal(3, result.BarrasNecesarias);
        Assert.Equal(40, result.TotalPiezas);
        Assert.NotNull(result.SobranteTotal);

        // 2 barras completas (16 piezas c/u) + 1 barra con 8 piezas
        // Sobrante barras completas: 2 * 32 = 64
        // Sobrante última barra: 6000 - 8*373 = 6000 - 2984 = 3016
        // Sobrante total: 64 + 3016 = 3080
        Assert.Equal(3080, result.SobranteTotal);
    }

    [Fact]
    public void ConMargen_ReducePiezasPorBarra()
    {
        // piezaEfectiva = 370 + 10 = 380, paso = 383
        // floor(6000/383) = 15 piezas
        var result = CorteSimpleService.Calcular(6000, 370, 3, margen: 10);

        Assert.Equal(15, result.PiezasPorBarra);
        Assert.Equal(5700, result.LongitudUtil);   // 15 * 380
        Assert.Equal(45, result.MaterialEnCortes);  // 15 * 3
        Assert.Equal(255, result.SobrantePorBarra); // 6000 - 15*383
    }

    [Fact]
    public void SinKerf_CalculaCorrecto()
    {
        // Sin kerf: floor(6000/370) = 16 piezas
        var result = CorteSimpleService.Calcular(6000, 370, anchoCorte: 0);

        Assert.Equal(16, result.PiezasPorBarra);
        Assert.Equal(0, result.MaterialEnCortes);
        Assert.Equal(80, result.SobrantePorBarra);  // 6000 - 16*370
    }

    [Fact]
    public void LargoBarraInvalido_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            CorteSimpleService.Calcular(0, 370, 3));
    }

    [Fact]
    public void LargoPiezaInvalido_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            CorteSimpleService.Calcular(6000, -1, 3));
    }

    [Fact]
    public void CantidadDeseadaNegativa_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            CorteSimpleService.Calcular(6000, 370, 3, cantidadDeseada: -40));
    }

    [Fact]
    public void CantidadDeseadaCero_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            CorteSimpleService.Calcular(6000, 370, 3, cantidadDeseada: 0));
    }
}
