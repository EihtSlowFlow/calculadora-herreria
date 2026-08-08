# Calculadora de Herrería — Inventario y Cortes

Aplicación de escritorio para Windows que permite gestionar materiales de herrería, calcular el aprovechamiento de barras y perfiles, y planificar cortes optimizados.

## Características del prototipo

- **Inventario básico**: registro de materiales lineales (ángulos, tubos, planchuelas) y chapas/placas.
- **Calculadora flotante**: ventana compacta con tres modos de cálculo:
  - Corte simple de piezas iguales.
  - Plan de cortes con múltiples medidas.
  - Desarrollo circular a partir de diámetro.
- **Plantillas de fabricación**: modelos reutilizables de conjuntos de piezas.
- **Confirmación de consumo**: el inventario solo se modifica con acción explícita del usuario.
- **Historial de movimientos**: registro de consumos, ingresos y ajustes.

## Tecnología

- C# / .NET 8
- WPF (Windows Presentation Foundation)
- SQLite (vía Entity Framework Core)
- Patrón MVVM (CommunityToolkit.Mvvm)

## Estructura del proyecto

```
calculadora-herreria/
├── src/
│   ├── CalculadoraHerreria.Core/      # Librería compartida (net8.0)
│   │   ├── Models/                    # Entidades del dominio
│   │   └── Data/                      # DbContext, seeder, acceso a datos
│   └── CalculadoraHerreria/           # Proyecto WPF (net8.0-windows)
│       ├── ViewModels/                # Lógica de presentación (MVVM)
│       ├── Views/                     # Ventanas y controles XAML
│       └── Services/                  # Lógica de negocio y cálculos
├── tests/
│   └── CalculadoraHerreria.Tests/     # Tests unitarios (xUnit)
├── docs/
│   └── PLAN_PROTOTIPO.md             # Documento de diseño
└── CalculadoraHerreria.sln           # Solución .NET
```

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (para desarrollo)
- Windows 10/11 (para ejecutar la aplicación WPF)

## Compilar y ejecutar

```bash
# Restaurar paquetes
dotnet restore

# Compilar
dotnet build

# Ejecutar (requiere Windows)
dotnet run --project src/CalculadoraHerreria

# Ejecutar tests (funciona en cualquier OS)
dotnet test
```

## Base de datos

La base de datos SQLite (`calculadora_herreria.db`) se crea automáticamente junto al ejecutable en el primer inicio. Incluye datos de ejemplo para facilitar la validación.

### Tablas

| Tabla | Descripción |
|-------|-------------|
| `Materiales` | Inventario de barras, perfiles y chapas |
| `Movimientos` | Historial de consumos, ingresos y ajustes |
| `Plantillas` | Modelos de fabricación reutilizables |
| `PlantillaPiezas` | Piezas que componen cada plantilla |
| `Configuraciones` | Preferencias de la aplicación (clave-valor) |

## Documentación

- [Plan del prototipo](docs/PLAN_PROTOTIPO.md)
