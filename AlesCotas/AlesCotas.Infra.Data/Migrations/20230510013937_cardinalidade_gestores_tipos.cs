using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class cardinalidade_gestores_tipos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ProdutoTipoGestor",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ProdutoTipoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoTipoGestor", x => new { x.ProdutoTipoId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_ProdutoTipoGestor_ProdutoTipo_ProdutoTipoId",
                        column: x => x.ProdutoTipoId,
                        principalTable: "ProdutoTipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutoTipoGestor_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoTipoGestor_UsuarioId",
                table: "ProdutoTipoGestor",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutoTipoGestor");

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
    }
}
