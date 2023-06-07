using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class prd_tipo_campos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProdutoTipoCampos_ProdutoTipoId",
                table: "ProdutoTipoCampos");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProdutoTipoCampos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                collation: "SQL_LATIN1_GENERAL_CP1_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoTipoCampos_ProdutoTipoId_Name",
                table: "ProdutoTipoCampos",
                columns: new[] { "ProdutoTipoId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProdutoTipoCampos_ProdutoTipoId_Name",
                table: "ProdutoTipoCampos");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProdutoTipoCampos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldCollation: "SQL_LATIN1_GENERAL_CP1_CI_AI");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoTipoCampos_ProdutoTipoId",
                table: "ProdutoTipoCampos",
                column: "ProdutoTipoId");
        }
    }
}
