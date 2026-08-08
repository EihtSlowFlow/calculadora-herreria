# Calculadora de Herrería — Inventario y Cortes

Aplicación de escritorio multiplataforma (Windows y Linux) que permite gestionar materiales de herrería, calcular el aprovechamiento de barras y perfiles, y planificar cortes optimizados.

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
- Avalonia UI (Framework multiplataforma)
- SQLite (vía Entity Framework Core)
- Patrón MVVM (CommunityToolkit.Mvvm)

## Estructura del proyecto

```
calculadora-herreria/
├── src/
│   ├── CalculadoraHerreria.Core/      # Librería compartida (net8.0)
│   │   ├── Models/                    # Entidades del dominio
│   │   ├── Data/                      # DbContext, seeder, acceso a datos
│   │   └── Data/Migrations/           # Migraciones de EF Core
│   └── CalculadoraHerreria/           # Proyecto Avalonia UI (net8.0)
│       ├── ViewModels/                # Lógica de presentación (MVVM)
│       ├── Views/                     # Ventanas y controles XAML (axaml)
│       └── Services/                  # Lógica de negocio y cálculos
├── tests/
│   └── CalculadoraHerreria.Tests/     # Tests unitarios (xUnit)
├── docs/
│   └── PLAN_PROTOTIPO.md             # Documento de diseño
└── CalculadoraHerreria.sln           # Solución .NET
```

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (para desarrollo)
- Windows 10/11 o Linux (Kubuntu, Ubuntu, etc.)

## Compilar y ejecutar

```bash
# Restaurar paquetes
dotnet restore

# Compilar
dotnet build

# Ejecutar tests
dotnet test

# Ejecutar aplicación
dotnet run --project src/CalculadoraHerreria

# Publicar (ejemplo para Windows x64 autocontenido)
dotnet publish src/CalculadoraHerreria -c Release -r win-x64 --self-contained true
```

## Base de datos

La base de datos SQLite (`calculadora_herreria.db`) se ubica en la carpeta de datos de usuario de la plataforma:
- **Windows**: `%LOCALAPPDATA%\CalculadoraHerreria\`
- **Linux**: `~/.local/share/CalculadoraHerreria/`

Se crean las tablas automáticamente al iniciar gracias a Entity Framework Core Migrations.

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
