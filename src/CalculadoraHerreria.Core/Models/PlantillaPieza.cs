using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalculadoraHerreria.Models;

/// <summary>
/// Una pieza dentro de una plantilla de fabricación.
/// </summary>
public class PlantillaPieza
{
    [Key]
    public int Id { get; set; }

    /// <summary>ID de la plantilla a la que pertenece.</summary>
    [Required]
    public int PlantillaId { get; set; }

    /// <summary>Navegación a la plantilla.</summary>
    [ForeignKey(nameof(PlantillaId))]
    public Plantilla? Plantilla { get; set; }

    /// <summary>Cantidad de esta pieza por unidad de producto.</summary>
    [Required]
    public int Cantidad { get; set; } = 1;

    /// <summary>Largo de la pieza en milímetros.</summary>
    [Required]
    public double LargoMm { get; set; }

    /// <summary>Descripción del uso de la pieza, ej: "Patas", "Lados largos".</summary>
    [MaxLength(200)]
    public string? DescripcionUso { get; set; }
}
