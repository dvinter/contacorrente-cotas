using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class fluxo_conta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fluxo_conta",
                columns: table => new
                {
                    GabineteId = table.Column<int>(type: "int", nullable: false),
                    Sequencia = table.Column<int>(type: "int", nullable: false),
                    MovimentoId = table.Column<int>(type: "int", nullable: false),
                    SequenciaMovimento = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataMovimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaldoAnterior = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaldoAtual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaldoAnteriorAno = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaldoAtualAno = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fluxo_conta", x => new { x.GabineteId, x.Sequencia });
                });

            migrationBuilder.CreateTable(
                name: "fluxo_conta_fila",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GabineteId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    DataMovimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MovimentoId = table.Column<int>(type: "int", nullable: false),
                    Erro = table.Column<bool>(type: "bit", nullable: false),
                    ErroMensagem = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true, collation: "SQL_LATIN1_GENERAL_CP1_CI_AI")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fluxo_conta_fila", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fluxo_conta_Ano_Mes",
                table: "fluxo_conta",
                columns: new[] { "Ano", "Mes" });

            migrationBuilder.CreateIndex(
                name: "IX_fluxo_conta_MovimentoId_SequenciaMovimento",
                table: "fluxo_conta",
                columns: new[] { "MovimentoId", "SequenciaMovimento" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fluxo_conta");

            migrationBuilder.DropTable(
                name: "fluxo_conta_fila");
        }
    }
}
