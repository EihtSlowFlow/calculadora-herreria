using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalculadoraHerreria.Models;

/// <summary>
/// Representa un material del inventario: barras, perfiles, chapas, etc.
/// </summary>
public class Material
{
    [Key]
    public int Id { get; set; }

    /// <summary>Nombre o descripción del material.</summary>
    [Required]
    [MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Familia: "Lineal" o "Chapa".</summary>
    [Required]
    [MaxLength(50)]
    public string Familia { get; set; } = string.Empty;

    /// <summary>Forma del perfil: Ángulo L, Perfil T, Planchuela, Tubo redondo, etc.</summary>
    [MaxLength(50)]
    public string? Forma { get; set; }

    /// <summary>Material base: Hierro, Acero inoxidable, Aluminio, etc.</summary>
    [MaxLength(100)]
    public string? MaterialBase { get; set; }

    /// <summary>Dimensiones descriptivas, ej: "40x40", "50x25x2".</summary>
    [MaxLength(100)]
    public string? Dimensiones { get; set; }

    /// <summary>Espesor en milímetros.</summary>
    public double? Espesor { get; set; }

    /// <summary>Unidad original del material: mm, cm, m, pulgadas.</summary>
    [Required]
    [MaxLength(20)]
    public string UnidadOriginal { get; set; } = "mm";

    /// <summary>Largo disponible en milímetros (para materiales lineales). Default: 6000 mm.</summary>
    public double? LargoDisponible { get; set; } = 6000;

    /// <summary>Ancho en milímetros (para chapas y placas).</summary>
    public double? Ancho { get; set; }

    /// <summary>Cantidad de unidades en stock.</summary>
    [Required]
    public int Cantidad { get; set; } = 0;

    /// <summary>Condición: "Completa" o "Sobrante".</summary>
    [Required]
    [MaxLength(20)]
    public string Condicion { get; set; } = "Completa";

    /// <summary>Observaciones adicionales.</summary>
    [MaxLength(500)]
    public string? Observaciones { get; set; }

    /// <summary>Área calculada automáticamente para chapas (Largo × Ancho) en mm².</summary>
    [NotMapped]
    public double? AreaCalculada =>
        (Familia == "Chapa" && LargoDisponible.HasValue && Ancho.HasValue)
            ? LargoDisponible.Value * Ancho.Value
            : null;
}
