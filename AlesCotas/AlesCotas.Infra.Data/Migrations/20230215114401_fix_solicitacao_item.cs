using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class fix_solicitacao_item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SolicitacaoId2",
                table: "SolicitacaoItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SolicitacaoId2",
                table: "SolicitacaoItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
