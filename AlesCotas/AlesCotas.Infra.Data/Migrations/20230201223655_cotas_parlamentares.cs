using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class cotas_parlamentares : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProdutoTipoId",
                table: "Solicitacao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumeroParcelas",
                table: "ProdutoTipo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cotas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, collation: "SQL_LATIN1_GENERAL_CP1_CI_AI"),
                    Lei = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, collation: "SQL_LATIN1_GENERAL_CP1_CI_AI"),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cotas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_ProdutoTipoId",
                table: "Solicitacao",
                column: "ProdutoTipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cotas_DataInicio",
                table: "Cotas",
                column: "DataInicio",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacao_ProdutoTipo_ProdutoTipoId",
                table: "Solicitacao",
                column: "ProdutoTipoId",
                principalTable: "ProdutoTipo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacao_ProdutoTipo_ProdutoTipoId",
                table: "Solicitacao");

            migrationBuilder.DropTable(
                name: "Cotas");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacao_ProdutoTipoId",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "ProdutoTipoId",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "NumeroParcelas",
                table: "ProdutoTipo");
        }
    }
}
