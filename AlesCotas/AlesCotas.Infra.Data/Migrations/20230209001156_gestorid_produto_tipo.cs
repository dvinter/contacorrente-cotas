using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class gestorid_produto_tipo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GestorId",
                table: "ProdutoTipo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoTipo_GestorId",
                table: "ProdutoTipo",
                column: "GestorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoTipo_Usuarios_GestorId",
                table: "ProdutoTipo",
                column: "GestorId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoTipo_Usuarios_GestorId",
                table: "ProdutoTipo");

            migrationBuilder.DropIndex(
                name: "IX_ProdutoTipo_GestorId",
                table: "ProdutoTipo");

            migrationBuilder.DropColumn(
                name: "GestorId",
                table: "ProdutoTipo");
        }
    }
}
