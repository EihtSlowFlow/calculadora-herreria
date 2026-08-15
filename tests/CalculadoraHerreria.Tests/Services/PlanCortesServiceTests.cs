using CalculadoraHerreria.Core.Services;
using Xunit;

namespace CalculadoraHerreria.Tests.Services;

public class PlanCortesServiceTests
{
    [Fact]
    public void CasoDocumentado_ProductoMesaBasica_40Unidades()
    {
        // Del doc: Producto unitario: 2×400mm + 2×600mm + 4×125mm
        // Para 40 unidades → 80×400 + 80×600 + 160×125
        var piezas = new List<PiezaSolicitada>
        {
            new() { Cantidad = 2, LargoMm = 600, DescripcionUso = "Lados largos" },
            new() { Cantidad = 2, LargoMm = 400, DescripcionUso = "Lados cortos" },
            new() { Cantidad = 4, LargoMm = 125, DescripcionUso = "Patas" }
        };

        var result = PlanCortesService.Calcular(
            piezas: piezas,
            cantidadProductos: 40,
            largoBarra: 6000,
            anchoCorte: 3);

        // Verificar que no hay barras excedidas
        foreach (var barra in result.Barras)
        {
            double total = barra.LongitudUtilizada + barra.MaterialEnCortes + barra.Sobrante;
            Assert.Equal(6000, total, 2);
            Assert.True(barra.Sobrante >= 0, $"Barra {barra.NumeroBarra} tiene sobrante negativo: {barra.Sobrante}");
        }

        // Verificar resumen de piezas
        Assert.Equal(3, result.ResumenPiezas.Count);

        var resumen600 = result.ResumenPiezas.First(r => r.LargoMm == 600);
        Assert.Equal(80, resumen600.CantidadTotal);

        var resumen400 = result.ResumenPiezas.First(r => r.LargoMm == 400);
        Assert.Equal(80, resumen400.CantidadTotal);

        var resumen125 = result.ResumenPiezas.First(r => r.LargoMm == 125);
        Assert.Equal(160, resumen125.CantidadTotal);

        // Verificar que todas las piezas fueron asignadas
        int totalPiezasAsignadas = result.Barras.Sum(b => b.Piezas.Count);
        Assert.Equal(320, totalPiezasAsignadas); // 80+80+160
    }

    [Fact]
    public void PiezasSimples_UnasMedidas_DistribucionValida()
    {
        var piezas = new List<PiezaSolicitada>
        {
            new() { Cantidad = 5, LargoMm = 1000, DescripcionUso = "Piezas largas" }
        };

        var result = PlanCortesService.Calcular(piezas, 1, 6000, 3);

        // 5 piezas de 1000mm en barras de 6000mm
        // Con kerf 3: cada pieza ocupa 1003mm, caben 5 por barra (5*1003=5015)
        Assert.Equal(1, result.BarrasNecesarias);
        Assert.Equal(5, result.Barras[0].Piezas.Count);
        Assert.True(result.Barras[0].Sobrante >= 0);
    }

    [Fact]
    public void SobranteMinimo_MarcaSobrantesReutilizables()
    {
        var piezas = new List<PiezaSolicitada>
        {
            new() { Cantidad = 1, LargoMm = 5500, DescripcionUso = "Pieza grande" }
        };

        // Con kerf 3mm: pieza ocupa 5503, sobrante = 497mm (> 100 mínimo)
        var result = PlanCortesService.Calcular(piezas, 1, 6000, 3, sobranteMinimoMm: 100);
        Assert.True(result.Barras[0].SobranteReutilizable);

        // Ahora con sobrante mínimo alto
        var result2 = PlanCortesService.Calcular(piezas, 1, 6000, 3, sobranteMinimoMm: 600);
        Assert.False(result2.Barras[0].SobranteReutilizable);
    }

    [Fact]
    public void FFD_OrdenaMayorAMenor_DistribucionEficiente()
    {
        // Piezas que, si se ordenan FFD, caben en menos barras
        var piezas = new List<PiezaSolicitada>
        {
            new() { Cantidad = 2, LargoMm = 3000, DescripcionUso = "Grandes" },
            new() { Cantidad = 4, LargoMm = 1400, DescripcionUso = "Medianas" }
        };

        var result = PlanCortesService.Calcular(piezas, 1, 6000, 3);

        // FFD: primero las de 3000, luego las de 1400
        // Barra 1: 3000+3+1400+3+1400+3 = 5809, sobrante 191
        // Barra 2: 3000+3+1400+3+1400+3 = 5809, sobrante 191
        // Caben en 2 barras
        Assert.Equal(2, result.BarrasNecesarias);
    }

    [Fact]
    public void PiezaMayorQueBarra_LanzaExcepcion()
    {
        var piezas = new List<PiezaSolicitada>
        {
            new() { Cantidad = 1, LargoMm = 7000, DescripcionUso = "Excesiva" }
        };

        Assert.Throws<ArgumentException>(() =>
            PlanCortesService.Calcular(piezas, 1, 6000, 3));
    }

    [Fact]
    public void ListaVacia_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            PlanCortesService.Calcular(new List<PiezaSolicitada>(), 1, 6000));
    }

    [Fact]
    public void CantidadProductosCero_LanzaExcepcion()
    {
        var piezas = new List<PiezaSolicitada>
        {
            new() { Cantidad = 1, LargoMm = 500, DescripcionUso = "Test" }
        };

        Assert.Throws<ArgumentException>(() =>
            PlanCortesService.Calcular(piezas, 0, 6000));
    }

    [Fact]
    public void NingunaBarraExcedida_EnCualquierDistribucion()
    {
        // Prueba con muchas piezas de distintos tamaños
        var piezas = new List<PiezaSolicitada>
        {
            new() { Cantidad = 10, LargoMm = 800, DescripcionUso = "Tipo A" },
            new() { Cantidad = 15, LargoMm = 350, DescripcionUso = "Tipo B" },
            new() { Cantidad = 20, LargoMm = 200, DescripcionUso = "Tipo C" },
            new() { Cantidad = 5, LargoMm = 1500, DescripcionUso = "Tipo D" }
        };

        var result = PlanCortesService.Calcular(piezas, 2, 6000, 3);

        // Verificar que NINGUNA barra está excedida
        foreach (var barra in result.Barras)
        {
            double totalOcupado = barra.LongitudUtilizada + barra.MaterialEnCortes;
            Assert.True(totalOcupado <= 6000,
                $"Barra {barra.NumeroBarra} excedida: {totalOcupado} mm > 6000 mm");
            Assert.True(barra.Sobrante >= 0,
                $"Barra {barra.NumeroBarra} tiene sobrante negativo: {barra.Sobrante}");
        }

        // Verificar total de piezas
        int totalEsperado = (10 + 15 + 20 + 5) * 2;
        int totalAsignado = result.Barras.Sum(b => b.Piezas.Count);
        Assert.Equal(totalEsperado, totalAsignado);
    }
}
