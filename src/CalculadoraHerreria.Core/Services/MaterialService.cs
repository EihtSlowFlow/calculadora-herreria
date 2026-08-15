using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculadoraHerreria.Data;
using CalculadoraHerreria.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraHerreria.Core.Services;

public class MaterialService : IMaterialService
{
    private readonly AppDbContext _context;

    public MaterialService(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Material>> ObtenerLinealesAsync(string? forma = null, string? condicion = null)
    {
        var query = _context.Materiales.Where(m => m.Familia == "Lineal");

        if (!string.IsNullOrWhiteSpace(forma))
        {
            query = query.Where(m => m.Forma == forma);
        }

        if (!string.IsNullOrWhiteSpace(condicion))
        {
            query = query.Where(m => m.Condicion == condicion);
        }

        return await query.ToListAsync();
    }

    public async Task<List<Material>> ObtenerChapasAsync(string? condicion = null)
    {
        var query = _context.Materiales.Where(m => m.Familia == "Chapa");

        if (!string.IsNullOrWhiteSpace(condicion))
        {
            query = query.Where(m => m.Condicion == condicion);
        }

        return await query.ToListAsync();
    }

    public async Task<Material> AgregarMaterialAsync(Material material)
    {
        if (material == null) throw new ArgumentNullException(nameof(material));
        ValidarMaterial(material);

        _context.Materiales.Add(material);
        await _context.SaveChangesAsync();
        return material;
    }

    public async Task ActualizarMaterialAsync(Material material)
    {
        if (material == null) throw new ArgumentNullException(nameof(material));
        ValidarMaterial(material);

        _context.Materiales.Update(material);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarMaterialAsync(int id)
    {
        var material = await _context.Materiales.FindAsync(id);
        if (material != null)
        {
            _context.Materiales.Remove(material);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Valida las reglas de negocio de un material antes de persistirlo.
    /// </summary>
    private static void ValidarMaterial(Material material)
    {
        // Familia válida
        if (!CatalogoMateriales.EsFamiliaValida(material.Familia))
            throw new ArgumentException(
                $"Familia '{material.Familia}' no es válida. Familias soportadas: {string.Join(", ", CatalogoMateriales.Familias)}",
                nameof(material));

        // Condición válida
        if (!CatalogoMateriales.EsCondicionValida(material.Condicion))
            throw new ArgumentException(
                $"Condición '{material.Condicion}' no es válida. Condiciones soportadas: {string.Join(", ", CatalogoMateriales.Condiciones)}",
                nameof(material));

        // Unidad válida
        if (!UnitConversionService.EsUnidadValida(material.UnidadOriginal))
            throw new ArgumentException(
                $"Unidad '{material.UnidadOriginal}' no es válida.",
                nameof(material));

        // Nombre requerido
        if (string.IsNullOrWhiteSpace(material.Nombre))
            throw new ArgumentException("El nombre del material es obligatorio.", nameof(material));

        // Cantidad no negativa
        if (material.Cantidad < 0)
            throw new ArgumentException("La cantidad no puede ser negativa.", nameof(material));

        // Validaciones específicas para materiales lineales
        if (material.Familia == CatalogoMateriales.FamiliaLineal)
        {
            // Forma requerida y válida para lineales
            if (string.IsNullOrWhiteSpace(material.Forma) || !CatalogoMateriales.EsFormaValida(material.Forma))
                throw new ArgumentException(
                    $"Forma '{material.Forma}' no es válida para materiales lineales. Formas soportadas: {string.Join(", ", CatalogoMateriales.FormasLineales)}",
                    nameof(material));

            // Largo disponible positivo
            if (!material.LargoDisponible.HasValue || material.LargoDisponible <= 0)
                throw new ArgumentException("El largo disponible debe ser mayor a 0 para materiales lineales.", nameof(material));
        }

        // Validaciones específicas para chapas
        if (material.Familia == CatalogoMateriales.FamiliaChapa)
        {
            if (!material.LargoDisponible.HasValue || material.LargoDisponible <= 0)
                throw new ArgumentException("El largo debe ser mayor a 0 para chapas.", nameof(material));

            if (!material.Ancho.HasValue || material.Ancho <= 0)
                throw new ArgumentException("El ancho debe ser mayor a 0 para chapas.", nameof(material));
        }

        // Espesor no negativo (si se proporciona)
        if (material.Espesor.HasValue && material.Espesor < 0)
            throw new ArgumentException("El espesor no puede ser negativo.", nameof(material));
    }
}
