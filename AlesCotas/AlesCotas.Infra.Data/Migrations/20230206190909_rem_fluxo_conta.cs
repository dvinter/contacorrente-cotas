using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class rem_fluxo_conta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FluxoContaFila");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "MovimentoConta",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                collation: "SQL_LATIN1_GENERAL_CP1_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldCollation: "SQL_LATIN1_GENERAL_CP1_CI_AI");

            migrationBuilder.AddColumn<int>(
                name: "NaturezaMovimento",
                table: "MovimentoConta",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NaturezaMovimento",
                table: "MovimentoConta");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "MovimentoConta",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                collation: "SQL_LATIN1_GENERAL_CP1_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true,
                oldCollation: "SQL_LATIN1_GENERAL_CP1_CI_AI");

            migrationBuilder.CreateTable(
                name: "FluxoContaFila",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataMovimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Erro = table.Column<bool>(type: "bit", nullable: false),
                    ErroMensagem = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false, collation: "SQL_LATIN1_GENERAL_CP1_CI_AI"),
                    GabineteId = table.Column<int>(type: "int", nullable: false),
                    MovimentoId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxoContaFila", x => x.Id);
                });
        }
    }
}
