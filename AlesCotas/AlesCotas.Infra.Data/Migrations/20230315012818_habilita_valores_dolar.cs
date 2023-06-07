using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class habilita_valores_dolar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HabilitaCotacaoDolar",
                table: "ProdutoTipo",
                newName: "HabilitaValoresDolar");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HabilitaValoresDolar",
                table: "ProdutoTipo",
                newName: "HabilitaCotacaoDolar");
        }
    }
}
