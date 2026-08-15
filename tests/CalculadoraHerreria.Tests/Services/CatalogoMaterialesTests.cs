using CalculadoraHerreria.Core.Services;
using Xunit;

namespace CalculadoraHerreria.Tests.Services;

public class CatalogoMaterialesTests
{
    [Fact]
    public void FormasLineales_ContieneSieteFormas()
    {
        Assert.Equal(7, CatalogoMateriales.FormasLineales.Count);
    }

    [Theory]
    [InlineData("Ángulo L")]
    [InlineData("Perfil T")]
    [InlineData("Planchuela")]
    [InlineData("Tubo redondo")]
    [InlineData("Tubo cuadrado")]
    [InlineData("Tubo rectangular")]
    [InlineData("Personalizado")]
    public void FormasLineales_ContieneTodasLasFormasEsperadas(string forma)
    {
        Assert.Contains(forma, CatalogoMateriales.FormasLineales);
    }

    [Theory]
    [InlineData("Ángulo L", true)]
    [InlineData("Tubo cuadrado", true)]
    [InlineData("Personalizado", true)]
    [InlineData("Inexistente", false)]
    [InlineData("", false)]
    public void EsFormaValida_RetornaResultadoCorrecto(string forma, bool esperado)
    {
        Assert.Equal(esperado, CatalogoMateriales.EsFormaValida(forma));
    }

    [Theory]
    [InlineData("Lineal", true)]
    [InlineData("Chapa", true)]
    [InlineData("Otra", false)]
    [InlineData("", false)]
    public void EsFamiliaValida_RetornaResultadoCorrecto(string familia, bool esperado)
    {
        Assert.Equal(esperado, CatalogoMateriales.EsFamiliaValida(familia));
    }

    [Fact]
    public void Condiciones_ContieneCompletaYSobrante()
    {
        Assert.Equal(2, CatalogoMateriales.Condiciones.Count);
        Assert.Contains("Completa", CatalogoMateriales.Condiciones);
        Assert.Contains("Sobrante", CatalogoMateriales.Condiciones);
    }

    [Theory]
    [InlineData("Completa", true)]
    [InlineData("Sobrante", true)]
    [InlineData("Rota", false)]
    public void EsCondicionValida_RetornaResultadoCorrecto(string condicion, bool esperado)
    {
        Assert.Equal(esperado, CatalogoMateriales.EsCondicionValida(condicion));
    }
}
