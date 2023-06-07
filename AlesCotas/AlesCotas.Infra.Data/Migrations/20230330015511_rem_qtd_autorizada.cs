using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class rem_qtd_autorizada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeAutorizada",
                table: "SolicitacaoItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "QuantidadeAutorizada",
                table: "SolicitacaoItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
