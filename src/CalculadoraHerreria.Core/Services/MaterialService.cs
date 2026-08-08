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
        
        _context.Materiales.Add(material);
        await _context.SaveChangesAsync();
        return material;
    }

    public async Task ActualizarMaterialAsync(Material material)
    {
        if (material == null) throw new ArgumentNullException(nameof(material));

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
}
