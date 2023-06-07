using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class parlamentar_gabinete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Produto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ParlamentarId",
                table: "Gabinetes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gabinetes_ParlamentarId",
                table: "Gabinetes",
                column: "ParlamentarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gabinetes_Parlamentares_ParlamentarId",
                table: "Gabinetes",
                column: "ParlamentarId",
                principalTable: "Parlamentares",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gabinetes_Parlamentares_ParlamentarId",
                table: "Gabinetes");

            migrationBuilder.DropIndex(
                name: "IX_Gabinetes_ParlamentarId",
                table: "Gabinetes");

            migrationBuilder.DropColumn(
                name: "ParlamentarId",
                table: "Gabinetes");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Produto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
