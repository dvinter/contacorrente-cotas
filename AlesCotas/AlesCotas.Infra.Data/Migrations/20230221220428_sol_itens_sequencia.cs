using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class sol_itens_sequencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SolicitacaoItem",
                table: "SolicitacaoItem");

            migrationBuilder.AddColumn<int>(
                name: "Sequencia",
                table: "SolicitacaoItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolicitacaoItem",
                table: "SolicitacaoItem",
                columns: new[] { "Sequencia", "SolicitacaoId" });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoItem_ProdutoId",
                table: "SolicitacaoItem",
                column: "ProdutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SolicitacaoItem",
                table: "SolicitacaoItem");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacaoItem_ProdutoId",
                table: "SolicitacaoItem");

            migrationBuilder.DropColumn(
                name: "Sequencia",
                table: "SolicitacaoItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolicitacaoItem",
                table: "SolicitacaoItem",
                columns: new[] { "ProdutoId", "SolicitacaoId" });
        }
    }
}
