using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class tabela_anexos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anexo",
                table: "Solicitacao");

            migrationBuilder.CreateTable(
                name: "SolicitacaoAnexos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Extensao = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    DataArquivo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tamanho = table.Column<long>(type: "bigint", nullable: false),
                    Dados = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    SolicitacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoAnexos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitacaoAnexos_Solicitacao_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "Solicitacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoAnexos_SolicitacaoId",
                table: "SolicitacaoAnexos",
                column: "SolicitacaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitacaoAnexos");

            migrationBuilder.AddColumn<string>(
                name: "Anexo",
                table: "Solicitacao",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
