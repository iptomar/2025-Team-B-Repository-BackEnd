using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPT_Teste.Migrations
{
    /// <inheritdoc />
    public partial class TurmaFKSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TurmaFK",
                table: "Horarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_TurmaFK",
                table: "Horarios",
                column: "TurmaFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Horarios_Turmas_TurmaFK",
                table: "Horarios",
                column: "TurmaFK",
                principalTable: "Turmas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horarios_Turmas_TurmaFK",
                table: "Horarios");

            migrationBuilder.DropIndex(
                name: "IX_Horarios_TurmaFK",
                table: "Horarios");

            migrationBuilder.DropColumn(
                name: "TurmaFK",
                table: "Horarios");
        }
    }
}
