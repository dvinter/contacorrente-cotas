using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class SolicitacaoAdiantamentoCota : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolicitacaoAdintamentoCota",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, collation: "SQL_LATIN1_GENERAL_CP1_CI_AI"),
                    MesReferente = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GabineteId = table.Column<int>(type: "int", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataResposta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioRespostaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoAdintamentoCota", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitacaoAdintamentoCota_Gabinetes_GabineteId",
                        column: x => x.GabineteId,
                        principalTable: "Gabinetes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoAdintamentoCota_Usuarios_UsuarioRespostaId",
                        column: x => x.UsuarioRespostaId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoAdintamentoCota_GabineteId",
                table: "SolicitacaoAdintamentoCota",
                column: "GabineteId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoAdintamentoCota_UsuarioRespostaId",
                table: "SolicitacaoAdintamentoCota",
                column: "UsuarioRespostaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitacaoAdintamentoCota");
        }
    }
}
