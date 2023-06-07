using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class remocao_entity_parlamentar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gabinetes_Parlamentares_ParlamentarId",
                table: "Gabinetes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacao_Parlamentares_ParlamentarId",
                table: "Solicitacao");

            migrationBuilder.DropTable(
                name: "Parlamentares");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacao_ParlamentarId",
                table: "Solicitacao");

            migrationBuilder.DropIndex(
                name: "IX_Gabinetes_ParlamentarId",
                table: "Gabinetes");

            migrationBuilder.DropColumn(
                name: "ParlamentarId",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "ParlamentarId",
                table: "Gabinetes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParlamentarId",
                table: "Solicitacao",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParlamentarId",
                table: "Gabinetes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Parlamentares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Matricula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, collation: "SQL_LATIN1_GENERAL_CP1_CI_AI")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parlamentares", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_ParlamentarId",
                table: "Solicitacao",
                column: "ParlamentarId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacao_Parlamentares_ParlamentarId",
                table: "Solicitacao",
                column: "ParlamentarId",
                principalTable: "Parlamentares",
                principalColumn: "Id");
        }
    }
}
