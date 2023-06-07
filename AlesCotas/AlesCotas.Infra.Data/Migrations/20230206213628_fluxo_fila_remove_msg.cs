using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class fluxo_fila_remove_msg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Erro",
                table: "fluxo_conta_fila");

            migrationBuilder.DropColumn(
                name: "ErroMensagem",
                table: "fluxo_conta_fila");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Erro",
                table: "fluxo_conta_fila",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ErroMensagem",
                table: "fluxo_conta_fila",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "",
                collation: "SQL_LATIN1_GENERAL_CP1_CI_AI");
        }
    }
}
