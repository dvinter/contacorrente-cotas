using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class colunas_solicitacoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Justificativa",
                table: "SolicitacaoHistorico",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataSolicitacao",
                table: "Solicitacao",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Solicitacao",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Solicitacao",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ParlamentarId",
                table: "Solicitacao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FluxoContaFila",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GabineteId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    DataMovimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MovimentoId = table.Column<int>(type: "int", nullable: false),
                    Erro = table.Column<bool>(type: "bit", nullable: false),
                    ErroMensagem = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false, collation: "SQL_LATIN1_GENERAL_CP1_CI_AI")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxoContaFila", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_ParlamentarId",
                table: "Solicitacao",
                column: "ParlamentarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacao_Parlamentares_ParlamentarId",
                table: "Solicitacao",
                column: "ParlamentarId",
                principalTable: "Parlamentares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacao_Parlamentares_ParlamentarId",
                table: "Solicitacao");

            migrationBuilder.DropTable(
                name: "FluxoContaFila");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacao_ParlamentarId",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "Justificativa",
                table: "SolicitacaoHistorico");

            migrationBuilder.DropColumn(
                name: "DataSolicitacao",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "ParlamentarId",
                table: "Solicitacao");
        }
    }
}
