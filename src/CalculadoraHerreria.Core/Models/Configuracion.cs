using System.ComponentModel.DataAnnotations;

namespace CalculadoraHerreria.Models;

/// <summary>
/// Almacena pares clave-valor para la configuración de la aplicación.
/// Ejemplos: posición de ventana, unidades preferidas, sobrante mínimo.
/// </summary>
public class Configuracion
{
    [Key]
    [MaxLength(100)]
    public string Clave { get; set; } = string.Empty;

    /// <summary>Valor de la configuración.</summary>
    [MaxLength(500)]
    public string? Valor { get; set; }
}
