using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class publicacoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Publicacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MesReferencia = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "SQL_LATIN1_GENERAL_CP1_CI_AI"),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    GabineteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publicacoes_Gabinetes_GabineteId",
                        column: x => x.GabineteId,
                        principalTable: "Gabinetes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Publicacoes_GabineteId",
                table: "Publicacoes",
                column: "GabineteId");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacoes_MesReferencia_GabineteId",
                table: "Publicacoes",
                columns: new[] { "MesReferencia", "GabineteId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Publicacoes");
        }
    }
}
