using CalculadoraHerreria.Core.Services;
using Xunit;

namespace CalculadoraHerreria.Tests.Services;

public class DesarrolloCircularServiceTests
{
    [Fact]
    public void Diametro100_SinMargen()
    {
        var result = DesarrolloCircularService.Calcular(diametro: 100);

        Assert.Equal(100 * Math.PI, result.Circunferencia, 4);
        Assert.Equal(0, result.Margen);
        Assert.Equal(100 * Math.PI, result.LongitudPieza, 4);
        Assert.Equal(100 * Math.PI, result.LongitudTotal, 4);
        Assert.Equal(1, result.Cantidad);
    }

    [Fact]
    public void Diametro200_ConMargen50()
    {
        var result = DesarrolloCircularService.Calcular(diametro: 200, margen: 50);

        double circEsperada = 200 * Math.PI;
        Assert.Equal(circEsperada, result.Circunferencia, 4);
        Assert.Equal(50, result.Margen);
        Assert.Equal(circEsperada + 50, result.LongitudPieza, 4);
        Assert.Equal(circEsperada + 50, result.LongitudTotal, 4);
    }

    [Fact]
    public void Diametro50_ConMargen10_Cantidad5()
    {
        var result = DesarrolloCircularService.Calcular(diametro: 50, margen: 10, cantidad: 5);

        double circEsperada = 50 * Math.PI;
        double longitudPiezaEsperada = circEsperada + 10;
        double longitudTotalEsperada = longitudPiezaEsperada * 5;

        Assert.Equal(circEsperada, result.Circunferencia, 4);
        Assert.Equal(10, result.Margen);
        Assert.Equal(longitudPiezaEsperada, result.LongitudPieza, 4);
        Assert.Equal(longitudTotalEsperada, result.LongitudTotal, 4);
        Assert.Equal(5, result.Cantidad);
    }

    [Fact]
    public void Diametro25punto4_EsUnaPulgada()
    {
        // 25.4 mm = 1 pulgada
        var result = DesarrolloCircularService.Calcular(diametro: 25.4);

        Assert.Equal(25.4 * Math.PI, result.Circunferencia, 4);
    }

    [Fact]
    public void Diametro300_ConMargen20_Cantidad10()
    {
        var result = DesarrolloCircularService.Calcular(300, 20, 10);

        double circ = 300 * Math.PI;
        Assert.Equal(circ, result.Circunferencia, 4);
        Assert.Equal(circ + 20, result.LongitudPieza, 4);
        Assert.Equal((circ + 20) * 10, result.LongitudTotal, 4);
    }

    [Fact]
    public void DiametroInvalido_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            DesarrolloCircularService.Calcular(diametro: 0));
    }

    [Fact]
    public void DiametroNegativo_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            DesarrolloCircularService.Calcular(diametro: -50));
    }

    [Fact]
    public void CantidadInvalida_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            DesarrolloCircularService.Calcular(diametro: 100, cantidad: 0));
    }

    [Fact]
    public void MargenNegativo_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            DesarrolloCircularService.Calcular(diametro: 100, margen: -5));
    }
}
