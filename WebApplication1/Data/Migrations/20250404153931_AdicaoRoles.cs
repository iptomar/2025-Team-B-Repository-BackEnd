using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cadeira",
                columns: table => new
                {
                    Id_cadeira = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_cadeira = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ECTS = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cadeira", x => x.Id_cadeira);
                });

            migrationBuilder.CreateTable(
                name: "Grau",
                columns: table => new
                {
                    Id_grau = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_grau = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grau", x => x.Id_grau);
                });

            migrationBuilder.CreateTable(
                name: "Instituicao",
                columns: table => new
                {
                    Id_instituicao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_instituicao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instituicao", x => x.Id_instituicao);
                });

            migrationBuilder.CreateTable(
                name: "Localidade",
                columns: table => new
                {
                    Id_localidade = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_localidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localidade", x => x.Id_localidade);
                });

            migrationBuilder.CreateTable(
                name: "Professor",
                columns: table => new
                {
                    Id_professor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Apelido = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professor", x => x.Id_professor);
                });

            migrationBuilder.CreateTable(
                name: "Tipologia",
                columns: table => new
                {
                    Id_tipologia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_tipologia = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipologia", x => x.Id_tipologia);
                });

            migrationBuilder.CreateTable(
                name: "Utilizador",
                columns: table => new
                {
                    Id_utilizador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizador", x => x.Id_utilizador);
                });

            migrationBuilder.CreateTable(
                name: "Curso",
                columns: table => new
                {
                    Id_curso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Grau = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curso", x => x.Id_curso);
                    table.ForeignKey(
                        name: "FK_Curso_Grau_Grau",
                        column: x => x.Grau,
                        principalTable: "Grau",
                        principalColumn: "Id_grau",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sala",
                columns: table => new
                {
                    Id_sala = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Localidade = table.Column<int>(type: "int", nullable: false),
                    Nome_sala = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sala", x => x.Id_sala);
                    table.ForeignKey(
                        name: "FK_Sala_Localidade_Localidade",
                        column: x => x.Localidade,
                        principalTable: "Localidade",
                        principalColumn: "Id_localidade",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Professor_Cadeira",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Id_professor = table.Column<int>(type: "int", nullable: false),
                    Id_cadeira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professor_Cadeira", x => new { x.ID, x.Id_professor, x.Id_cadeira });
                    table.ForeignKey(
                        name: "FK_Professor_Cadeira_Cadeira_Id_cadeira",
                        column: x => x.Id_cadeira,
                        principalTable: "Cadeira",
                        principalColumn: "Id_cadeira",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Professor_Cadeira_Professor_Id_professor",
                        column: x => x.Id_professor,
                        principalTable: "Professor",
                        principalColumn: "Id_professor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tipologia_Cadeira",
                columns: table => new
                {
                    Id_tipologia = table.Column<int>(type: "int", nullable: false),
                    Id_cadeira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipologia_Cadeira", x => new { x.Id_tipologia, x.Id_cadeira });
                    table.ForeignKey(
                        name: "FK_Tipologia_Cadeira_Cadeira_Id_cadeira",
                        column: x => x.Id_cadeira,
                        principalTable: "Cadeira",
                        principalColumn: "Id_cadeira",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tipologia_Cadeira_Tipologia_Id_tipologia",
                        column: x => x.Id_tipologia,
                        principalTable: "Tipologia",
                        principalColumn: "Id_tipologia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Curso_Cadeira",
                columns: table => new
                {
                    Id_curso = table.Column<int>(type: "int", nullable: false),
                    Id_cadeira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curso_Cadeira", x => new { x.Id_curso, x.Id_cadeira });
                    table.ForeignKey(
                        name: "FK_Curso_Cadeira_Cadeira_Id_cadeira",
                        column: x => x.Id_cadeira,
                        principalTable: "Cadeira",
                        principalColumn: "Id_cadeira",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Curso_Cadeira_Curso_Id_curso",
                        column: x => x.Id_curso,
                        principalTable: "Curso",
                        principalColumn: "Id_curso",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Curso_Instituicao",
                columns: table => new
                {
                    Id_curso = table.Column<int>(type: "int", nullable: false),
                    Id_instituicao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curso_Instituicao", x => new { x.Id_curso, x.Id_instituicao });
                    table.ForeignKey(
                        name: "FK_Curso_Instituicao_Curso_Id_curso",
                        column: x => x.Id_curso,
                        principalTable: "Curso",
                        principalColumn: "Id_curso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Curso_Instituicao_Instituicao_Id_instituicao",
                        column: x => x.Id_instituicao,
                        principalTable: "Instituicao",
                        principalColumn: "Id_instituicao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Horario",
                columns: table => new
                {
                    Id_horario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Curso = table.Column<int>(type: "int", nullable: false),
                    Data_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    Data_fim = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horario", x => x.Id_horario);
                    table.ForeignKey(
                        name: "FK_Horario_Curso_Curso",
                        column: x => x.Curso,
                        principalTable: "Curso",
                        principalColumn: "Id_curso",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turma",
                columns: table => new
                {
                    Letra_turma = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Ano_academico = table.Column<int>(type: "int", nullable: false),
                    Semestre = table.Column<int>(type: "int", nullable: false),
                    Curso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turma", x => new { x.Letra_turma, x.Ano_academico, x.Semestre });
                    table.ForeignKey(
                        name: "FK_Turma_Curso_Curso",
                        column: x => x.Curso,
                        principalTable: "Curso",
                        principalColumn: "Id_curso",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bloco",
                columns: table => new
                {
                    Id_bloco = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hora_inicio = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Hora_fim = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Sala = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bloco", x => x.Id_bloco);
                    table.ForeignKey(
                        name: "FK_Bloco_Sala_Sala",
                        column: x => x.Sala,
                        principalTable: "Sala",
                        principalColumn: "Id_sala",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registo",
                columns: table => new
                {
                    Id_registo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Utilizador = table.Column<int>(type: "int", nullable: false),
                    Horario = table.Column<int>(type: "int", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registo", x => x.Id_registo);
                    table.ForeignKey(
                        name: "FK_Registo_Horario_Horario",
                        column: x => x.Horario,
                        principalTable: "Horario",
                        principalColumn: "Id_horario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registo_Utilizador_Utilizador",
                        column: x => x.Utilizador,
                        principalTable: "Utilizador",
                        principalColumn: "Id_utilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bloco_Horario",
                columns: table => new
                {
                    Id_bloco = table.Column<int>(type: "int", nullable: false),
                    Id_horario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bloco_Horario", x => new { x.Id_bloco, x.Id_horario });
                    table.ForeignKey(
                        name: "FK_Bloco_Horario_Bloco_Id_bloco",
                        column: x => x.Id_bloco,
                        principalTable: "Bloco",
                        principalColumn: "Id_bloco",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bloco_Horario_Horario_Id_horario",
                        column: x => x.Id_horario,
                        principalTable: "Horario",
                        principalColumn: "Id_horario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bloco_Sala",
                table: "Bloco",
                column: "Sala");

            migrationBuilder.CreateIndex(
                name: "IX_Bloco_Horario_Id_horario",
                table: "Bloco_Horario",
                column: "Id_horario");

            migrationBuilder.CreateIndex(
                name: "IX_Curso_Grau",
                table: "Curso",
                column: "Grau");

            migrationBuilder.CreateIndex(
                name: "IX_Curso_Cadeira_Id_cadeira",
                table: "Curso_Cadeira",
                column: "Id_cadeira");

            migrationBuilder.CreateIndex(
                name: "IX_Curso_Instituicao_Id_instituicao",
                table: "Curso_Instituicao",
                column: "Id_instituicao");

            migrationBuilder.CreateIndex(
                name: "IX_Horario_Curso",
                table: "Horario",
                column: "Curso");

            migrationBuilder.CreateIndex(
                name: "IX_Professor_Cadeira_Id_cadeira",
                table: "Professor_Cadeira",
                column: "Id_cadeira");

            migrationBuilder.CreateIndex(
                name: "IX_Professor_Cadeira_Id_professor",
                table: "Professor_Cadeira",
                column: "Id_professor");

            migrationBuilder.CreateIndex(
                name: "IX_Registo_Horario",
                table: "Registo",
                column: "Horario");

            migrationBuilder.CreateIndex(
                name: "IX_Registo_Utilizador",
                table: "Registo",
                column: "Utilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Sala_Localidade",
                table: "Sala",
                column: "Localidade");

            migrationBuilder.CreateIndex(
                name: "IX_Tipologia_Cadeira_Id_cadeira",
                table: "Tipologia_Cadeira",
                column: "Id_cadeira");

            migrationBuilder.CreateIndex(
                name: "IX_Turma_Curso",
                table: "Turma",
                column: "Curso");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bloco_Horario");

            migrationBuilder.DropTable(
                name: "Curso_Cadeira");

            migrationBuilder.DropTable(
                name: "Curso_Instituicao");

            migrationBuilder.DropTable(
                name: "Professor_Cadeira");

            migrationBuilder.DropTable(
                name: "Registo");

            migrationBuilder.DropTable(
                name: "Tipologia_Cadeira");

            migrationBuilder.DropTable(
                name: "Turma");

            migrationBuilder.DropTable(
                name: "Bloco");

            migrationBuilder.DropTable(
                name: "Instituicao");

            migrationBuilder.DropTable(
                name: "Professor");

            migrationBuilder.DropTable(
                name: "Horario");

            migrationBuilder.DropTable(
                name: "Utilizador");

            migrationBuilder.DropTable(
                name: "Cadeira");

            migrationBuilder.DropTable(
                name: "Tipologia");

            migrationBuilder.DropTable(
                name: "Sala");

            migrationBuilder.DropTable(
                name: "Curso");

            migrationBuilder.DropTable(
                name: "Localidade");

            migrationBuilder.DropTable(
                name: "Grau");
        }
    }
}
