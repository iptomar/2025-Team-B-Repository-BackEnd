using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPT_Teste.Migrations
{
    /// <inheritdoc />
    public partial class HorariosMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    Fim = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BlocosHorarios",
                columns: table => new
                {
                    BlocosId = table.Column<int>(type: "int", nullable: false),
                    HorariosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlocosHorarios", x => new { x.BlocosId, x.HorariosId });
                    table.ForeignKey(
                        name: "FK_BlocosHorarios_Blocos_BlocosId",
                        column: x => x.BlocosId,
                        principalTable: "Blocos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlocosHorarios_Horarios_HorariosId",
                        column: x => x.HorariosId,
                        principalTable: "Horarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BlocosHorarios_HorariosId",
                table: "BlocosHorarios",
                column: "HorariosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlocosHorarios");

            migrationBuilder.DropTable(
                name: "Horarios");
        }
    }
}
