namespace CalculadoraHerreria.Core.Services;

/// <summary>
/// Catálogo estático de familias, formas y condiciones de materiales.
/// Define las constantes del dominio para materiales lineales y chapas.
/// </summary>
public static class CatalogoMateriales
{
    // --- Familias ---
    public const string FamiliaLineal = "Lineal";
    public const string FamiliaChapa = "Chapa";

    // --- Formas de materiales lineales ---
    public const string FormaAnguloL = "Ángulo L";
    public const string FormaPerfilT = "Perfil T";
    public const string FormaPlanchuela = "Planchuela";
    public const string FormaTuboRedondo = "Tubo redondo";
    public const string FormaTuboCuadrado = "Tubo cuadrado";
    public const string FormaTuboRectangular = "Tubo rectangular";
    public const string FormaPersonalizado = "Personalizado";

    // --- Condiciones ---
    public const string CondicionCompleta = "Completa";
    public const string CondicionSobrante = "Sobrante";

    /// <summary>Lista de todas las formas de materiales lineales válidas.</summary>
    public static readonly IReadOnlyList<string> FormasLineales = new[]
    {
        FormaAnguloL,
        FormaPerfilT,
        FormaPlanchuela,
        FormaTuboRedondo,
        FormaTuboCuadrado,
        FormaTuboRectangular,
        FormaPersonalizado
    };

    /// <summary>Lista de todas las familias de materiales válidas.</summary>
    public static readonly IReadOnlyList<string> Familias = new[]
    {
        FamiliaLineal,
        FamiliaChapa
    };

    /// <summary>Lista de todas las condiciones válidas de un material.</summary>
    public static readonly IReadOnlyList<string> Condiciones = new[]
    {
        CondicionCompleta,
        CondicionSobrante
    };

    /// <summary>Verifica si una forma de material lineal es válida.</summary>
    public static bool EsFormaValida(string forma) =>
        FormasLineales.Contains(forma);

    /// <summary>Verifica si una familia de material es válida.</summary>
    public static bool EsFamiliaValida(string familia) =>
        Familias.Contains(familia);

    /// <summary>Verifica si una condición de material es válida.</summary>
    public static bool EsCondicionValida(string condicion) =>
        Condiciones.Contains(condicion);
}
