using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class removido_parcelamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Parcelas",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "NumeroParcelas",
                table: "ProdutoTipo");

            migrationBuilder.AddColumn<bool>(
                name: "PermitirRecorrencia",
                table: "ProdutoTipo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermitirRecorrencia",
                table: "ProdutoTipo");

            migrationBuilder.AddColumn<int>(
                name: "Parcelas",
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
        }
    }
}
