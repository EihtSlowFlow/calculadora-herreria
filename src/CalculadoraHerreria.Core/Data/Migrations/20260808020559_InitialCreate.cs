using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalculadoraHerreria.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuraciones",
                columns: table => new
                {
                    Clave = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Valor = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuraciones", x => x.Clave);
                });

            migrationBuilder.CreateTable(
                name: "Materiales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Familia = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Forma = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    MaterialBase = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Dimensiones = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Espesor = table.Column<double>(type: "REAL", nullable: true),
                    UnidadOriginal = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    LargoDisponible = table.Column<double>(type: "REAL", nullable: true),
                    Ancho = table.Column<double>(type: "REAL", nullable: true),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    Condicion = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materiales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plantillas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plantillas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movimientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CantidadAnterior = table.Column<int>(type: "INTEGER", nullable: false),
                    CantidadPosterior = table.Column<int>(type: "INTEGER", nullable: false),
                    SobranteMm = table.Column<double>(type: "REAL", nullable: true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movimientos_Materiales_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantillaPiezas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlantillaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    LargoMm = table.Column<double>(type: "REAL", nullable: false),
                    DescripcionUso = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantillaPiezas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantillaPiezas_Plantillas_PlantillaId",
                        column: x => x.PlantillaId,
                        principalTable: "Plantillas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_Condicion",
                table: "Materiales",
                column: "Condicion");

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_Familia",
                table: "Materiales",
                column: "Familia");

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_Forma",
                table: "Materiales",
                column: "Forma");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_Fecha",
                table: "Movimientos",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_MaterialId",
                table: "Movimientos",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantillaPiezas_PlantillaId",
                table: "PlantillaPiezas",
                column: "PlantillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuraciones");

            migrationBuilder.DropTable(
                name: "Movimientos");

            migrationBuilder.DropTable(
                name: "PlantillaPiezas");

            migrationBuilder.DropTable(
                name: "Materiales");

            migrationBuilder.DropTable(
                name: "Plantillas");
        }
    }
}
