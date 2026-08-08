using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalculadoraHerreria.Models;

/// <summary>
/// Registra un movimiento de inventario: consumo, ingreso o ajuste.
/// </summary>
public class Movimiento
{
    [Key]
    public int Id { get; set; }

    /// <summary>ID del material afectado.</summary>
    [Required]
    public int MaterialId { get; set; }

    /// <summary>Navegación al material.</summary>
    [ForeignKey(nameof(MaterialId))]
    public Material? Material { get; set; }

    /// <summary>Tipo de movimiento: "Consumo", "Ingreso", "Ajuste".</summary>
    [Required]
    [MaxLength(20)]
    public string Tipo { get; set; } = string.Empty;

    /// <summary>Cantidad antes del movimiento.</summary>
    public int CantidadAnterior { get; set; }

    /// <summary>Cantidad después del movimiento.</summary>
    public int CantidadPosterior { get; set; }

    /// <summary>Largo del sobrante registrado en mm (si aplica).</summary>
    public double? SobranteMm { get; set; }

    /// <summary>Fecha y hora del movimiento.</summary>
    [Required]
    public DateTime Fecha { get; set; } = DateTime.Now;

    /// <summary>Observaciones del movimiento.</summary>
    [MaxLength(500)]
    public string? Observaciones { get; set; }
}
