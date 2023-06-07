using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class solicitacao_campos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SolicitacaoId2",
                table: "SolicitacaoItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SolicitacaoId2",
                table: "SolicitacaoHistorico",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "Solicitacao",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Solicitacao",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Largura",
                table: "ProdutoTipoCampos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SolicitacaoCampos",
                columns: table => new
                {
                    SolicitacaoId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, collation: "SQL_LATIN1_GENERAL_CP1_CI_AI"),
                    TipoCampo = table.Column<int>(type: "int", nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Largura = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "SQL_LATIN1_GENERAL_CP1_CI_AI")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoCampos", x => new { x.Name, x.SolicitacaoId });
                    table.ForeignKey(
                        name: "FK_SolicitacaoCampos_Solicitacao_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoCampos_SolicitacaoId",
                table: "SolicitacaoCampos",
                column: "SolicitacaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitacaoCampos");

            migrationBuilder.DropColumn(
                name: "SolicitacaoId2",
                table: "SolicitacaoItem");

            migrationBuilder.DropColumn(
                name: "SolicitacaoId2",
                table: "SolicitacaoHistorico");

            migrationBuilder.DropColumn(
                name: "Largura",
                table: "ProdutoTipoCampos");

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "Solicitacao",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Solicitacao",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
