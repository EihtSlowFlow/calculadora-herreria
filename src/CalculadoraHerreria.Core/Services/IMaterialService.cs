using System.Collections.Generic;
using System.Threading.Tasks;
using CalculadoraHerreria.Models;

namespace CalculadoraHerreria.Core.Services;

public interface IMaterialService
{
    /// <summary>
    /// Obtiene todos los materiales lineales (barras, perfiles), opcionalmente filtrados.
    /// </summary>
    Task<List<Material>> ObtenerLinealesAsync(string? forma = null, string? condicion = null);

    /// <summary>
    /// Obtiene todas las chapas/placas, opcionalmente filtradas.
    /// </summary>
    Task<List<Material>> ObtenerChapasAsync(string? condicion = null);

    Task<Material> AgregarMaterialAsync(Material material);
    Task ActualizarMaterialAsync(Material material);
    Task EliminarMaterialAsync(int id);
}
