using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class incrementoquantidade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnidadeFracionada",
                table: "ProdutoTipo");

            migrationBuilder.AddColumn<decimal>(
                name: "IncrementoQuantidade",
                table: "ProdutoTipo",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncrementoQuantidade",
                table: "ProdutoTipo");

            migrationBuilder.AddColumn<bool>(
                name: "UnidadeFracionada",
                table: "ProdutoTipo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
