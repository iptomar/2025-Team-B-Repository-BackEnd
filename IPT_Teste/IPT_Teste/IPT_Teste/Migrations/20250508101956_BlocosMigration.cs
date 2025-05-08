using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPT_Teste.Migrations
{
    /// <inheritdoc />
    public partial class BlocosMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blocos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Hora_Inicio = table.Column<TimeOnly>(type: "time(6)", nullable: false),
                    SalaFK = table.Column<int>(type: "int", nullable: false),
                    TipologiaFK = table.Column<int>(type: "int", nullable: false),
                    CadeiraFK = table.Column<int>(type: "int", nullable: false),
                    AulaFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blocos_Aulas_AulaFK",
                        column: x => x.AulaFK,
                        principalTable: "Aulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Blocos_Cadeiras_CadeiraFK",
                        column: x => x.CadeiraFK,
                        principalTable: "Cadeiras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Blocos_Salas_SalaFK",
                        column: x => x.SalaFK,
                        principalTable: "Salas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Blocos_Tipologias_TipologiaFK",
                        column: x => x.TipologiaFK,
                        principalTable: "Tipologias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Blocos_AulaFK",
                table: "Blocos",
                column: "AulaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Blocos_CadeiraFK",
                table: "Blocos",
                column: "CadeiraFK");

            migrationBuilder.CreateIndex(
                name: "IX_Blocos_SalaFK",
                table: "Blocos",
                column: "SalaFK");

            migrationBuilder.CreateIndex(
                name: "IX_Blocos_TipologiaFK",
                table: "Blocos",
                column: "TipologiaFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blocos");
        }
    }
}
