using System.Threading.Tasks;
using CalculadoraHerreria.Data;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraHerreria.Core.Services;

/// <summary>
/// Servicio para gestionar la configuración de sobrante mínimo reutilizable.
/// El valor se almacena en la tabla Configuracion con la clave "SobranteMinimoMm".
/// </summary>
public class SobranteMinimoService
{
    public const string ClaveSobranteMinimo = "SobranteMinimoMm";
    public const double ValorPorDefecto = 100.0;

    private readonly AppDbContext _context;

    public SobranteMinimoService(AppDbContext context)
    {
        _context = context ?? throw new System.ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Obtiene el sobrante mínimo configurado (en mm).
    /// Devuelve el valor por defecto si no está configurado.
    /// </summary>
    public async Task<double> ObtenerSobranteMinimoAsync()
    {
        var config = await _context.Configuraciones.FindAsync(ClaveSobranteMinimo);
        if (config != null && double.TryParse(config.Valor, out double valor) && valor >= 0)
        {
            return valor;
        }
        return ValorPorDefecto;
    }

    /// <summary>
    /// Actualiza el sobrante mínimo configurado.
    /// </summary>
    public async Task ActualizarSobranteMinimoAsync(double valorMm)
    {
        if (valorMm < 0)
            throw new System.ArgumentException("El sobrante mínimo no puede ser negativo.", nameof(valorMm));

        var config = await _context.Configuraciones.FindAsync(ClaveSobranteMinimo);
        if (config != null)
        {
            config.Valor = valorMm.ToString();
            _context.Configuraciones.Update(config);
        }
        else
        {
            _context.Configuraciones.Add(new Models.Configuracion
            {
                Clave = ClaveSobranteMinimo,
                Valor = valorMm.ToString()
            });
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Determina si un sobrante es reutilizable según la configuración actual.
    /// </summary>
    public async Task<bool> EsSobranteReutilizableAsync(double sobranteMm)
    {
        var minimo = await ObtenerSobranteMinimoAsync();
        return sobranteMm >= minimo;
    }
}
