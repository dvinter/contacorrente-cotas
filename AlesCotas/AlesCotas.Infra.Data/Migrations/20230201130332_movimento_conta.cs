using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class movimento_conta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CancelamentoSolicitacaoAutomatico",
                table: "ProdutoTipo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MovimentoConta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false, collation: "SQL_LATIN1_GENERAL_CP1_CI_AI"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    GabineteId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentoConta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimentoConta_Gabinetes_GabineteId",
                        column: x => x.GabineteId,
                        principalTable: "Gabinetes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovimentoConta_GabineteId",
                table: "MovimentoConta",
                column: "GabineteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimentoConta");

            migrationBuilder.DropColumn(
                name: "CancelamentoSolicitacaoAutomatico",
                table: "ProdutoTipo");
        }
    }
}
