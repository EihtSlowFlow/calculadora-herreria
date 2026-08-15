using CalculadoraHerreria.Core.Services;
using Xunit;

namespace CalculadoraHerreria.Tests.Services;

public class UnitConversionServiceTests
{
    // --- Round-trip conversions ---

    [Theory]
    [InlineData(6000, "mm", "cm")]
    [InlineData(6000, "mm", "m")]
    [InlineData(6000, "mm", "pulgadas")]
    [InlineData(150, "cm", "pulgadas")]
    [InlineData(2.5, "m", "cm")]
    public void RoundTrip_SinPerdidaDePrecision(double valor, string unidadOrigen, string unidadIntermedia)
    {
        var intermedio = UnitConversionService.Convertir(valor, unidadOrigen, unidadIntermedia);
        var resultado = UnitConversionService.Convertir(intermedio, unidadIntermedia, unidadOrigen);

        Assert.Equal(valor, resultado, 8); // 8 decimales de precisión
    }

    // --- Conversiones específicas ---

    [Fact]
    public void ConvertirAMm_6000mm_Es6000()
    {
        Assert.Equal(6000, UnitConversionService.ConvertirAMm(6000, "mm"));
    }

    [Fact]
    public void Convertir_6000mmAMetros_Es6()
    {
        Assert.Equal(6.0, UnitConversionService.Convertir(6000, "mm", "m"));
    }

    [Fact]
    public void Convertir_1PulgadaAMm_Es25punto4()
    {
        Assert.Equal(25.4, UnitConversionService.ConvertirAMm(1, "pulgadas"));
    }

    [Fact]
    public void Convertir_100cmAMetros_Es1()
    {
        Assert.Equal(1.0, UnitConversionService.Convertir(100, "cm", "m"));
    }

    [Fact]
    public void Convertir_1mACm_Es100()
    {
        Assert.Equal(100.0, UnitConversionService.Convertir(1, "m", "cm"));
    }

    // --- Formateo para pantalla ---

    [Fact]
    public void FormatearParaPantalla_1234punto56_FormatoEspanol()
    {
        var resultado = UnitConversionService.FormatearParaPantalla(1234.56);
        Assert.Equal("1.234,56", resultado);
    }

    [Fact]
    public void FormatearParaPantalla_0punto5_DosDecimales()
    {
        var resultado = UnitConversionService.FormatearParaPantalla(0.5);
        Assert.Equal("0,50", resultado);
    }

    [Fact]
    public void FormatearParaPantalla_0_EsCero()
    {
        var resultado = UnitConversionService.FormatearParaPantalla(0);
        Assert.Equal("0,00", resultado);
    }

    // --- Validación de unidades ---

    [Theory]
    [InlineData("mm", true)]
    [InlineData("cm", true)]
    [InlineData("m", true)]
    [InlineData("pulgadas", true)]
    [InlineData("pies", false)]
    [InlineData("", false)]
    [InlineData("MM", false)]
    public void EsUnidadValida_RetornaResultadoCorrecto(string unidad, bool esperado)
    {
        Assert.Equal(esperado, UnitConversionService.EsUnidadValida(unidad));
    }

    [Fact]
    public void ConvertirAMm_UnidadInvalida_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            UnitConversionService.ConvertirAMm(100, "pies"));
    }

    [Fact]
    public void ConvertirDesdeMm_UnidadInvalida_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            UnitConversionService.ConvertirDesdeMm(100, "yardas"));
    }
}
