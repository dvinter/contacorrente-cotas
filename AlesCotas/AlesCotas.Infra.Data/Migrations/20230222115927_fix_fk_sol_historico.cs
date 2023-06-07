using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class fix_fk_sol_historico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SolicitacaoId2",
                table: "SolicitacaoHistorico");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SolicitacaoId2",
                table: "SolicitacaoHistorico",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
