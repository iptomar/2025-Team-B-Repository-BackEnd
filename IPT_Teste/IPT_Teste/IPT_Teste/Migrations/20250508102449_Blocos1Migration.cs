using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IPT_Teste.Migrations
{
    /// <inheritdoc />
    public partial class Blocos1Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocos_Cadeiras_CadeiraFK",
                table: "Blocos");

            migrationBuilder.DropForeignKey(
                name: "FK_Blocos_Tipologias_TipologiaFK",
                table: "Blocos");

            migrationBuilder.DropIndex(
                name: "IX_Blocos_CadeiraFK",
                table: "Blocos");

            migrationBuilder.DropIndex(
                name: "IX_Blocos_TipologiaFK",
                table: "Blocos");

            migrationBuilder.DropColumn(
                name: "CadeiraFK",
                table: "Blocos");

            migrationBuilder.DropColumn(
                name: "TipologiaFK",
                table: "Blocos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CadeiraFK",
                table: "Blocos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipologiaFK",
                table: "Blocos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Blocos_CadeiraFK",
                table: "Blocos",
                column: "CadeiraFK");

            migrationBuilder.CreateIndex(
                name: "IX_Blocos_TipologiaFK",
                table: "Blocos",
                column: "TipologiaFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Blocos_Cadeiras_CadeiraFK",
                table: "Blocos",
                column: "CadeiraFK",
                principalTable: "Cadeiras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Blocos_Tipologias_TipologiaFK",
                table: "Blocos",
                column: "TipologiaFK",
                principalTable: "Tipologias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
