using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class sol_parlamentar_null : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacao_Parlamentares_ParlamentarId",
                table: "Solicitacao");

            migrationBuilder.AlterColumn<int>(
                name: "ParlamentarId",
                table: "Solicitacao",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacao_Parlamentares_ParlamentarId",
                table: "Solicitacao",
                column: "ParlamentarId",
                principalTable: "Parlamentares",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacao_Parlamentares_ParlamentarId",
                table: "Solicitacao");

            migrationBuilder.AlterColumn<int>(
                name: "ParlamentarId",
                table: "Solicitacao",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacao_Parlamentares_ParlamentarId",
                table: "Solicitacao",
                column: "ParlamentarId",
                principalTable: "Parlamentares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
