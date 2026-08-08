using System.ComponentModel.DataAnnotations;

namespace CalculadoraHerreria.Models;

/// <summary>
/// Plantilla o modelo de fabricación reutilizable.
/// Define las piezas necesarias para fabricar un producto.
/// </summary>
public class Plantilla
{
    [Key]
    public int Id { get; set; }

    /// <summary>Nombre del producto o conjunto.</summary>
    [Required]
    [MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción opcional.</summary>
    [MaxLength(500)]
    public string? Descripcion { get; set; }

    /// <summary>Fecha de creación.</summary>
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    /// <summary>Piezas que componen esta plantilla.</summary>
    public List<PlantillaPieza> Piezas { get; set; } = new();
}
