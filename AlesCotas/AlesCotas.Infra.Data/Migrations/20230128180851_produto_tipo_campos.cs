using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class produto_tipo_campos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HabilitaAnexos",
                table: "ProdutoTipo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProtudoTipoCampos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoCampo = table.Column<int>(type: "int", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    ProdutoTipoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProtudoTipoCampos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProtudoTipoCampos_ProdutoTipo_ProdutoTipoId",
                        column: x => x.ProdutoTipoId,
                        principalTable: "ProdutoTipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProtudoTipoCampos_ProdutoTipoId",
                table: "ProtudoTipoCampos",
                column: "ProdutoTipoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProtudoTipoCampos");

            migrationBuilder.DropColumn(
                name: "HabilitaAnexos",
                table: "ProdutoTipo");
        }
    }
}
