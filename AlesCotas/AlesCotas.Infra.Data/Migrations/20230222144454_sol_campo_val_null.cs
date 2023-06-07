using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class sol_campo_val_null : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Valor",
                table: "SolicitacaoCampos",
                type: "nvarchar(max)",
                nullable: true,
                collation: "SQL_LATIN1_GENERAL_CP1_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldCollation: "SQL_LATIN1_GENERAL_CP1_CI_AI");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Valor",
                table: "SolicitacaoCampos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                collation: "SQL_LATIN1_GENERAL_CP1_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldCollation: "SQL_LATIN1_GENERAL_CP1_CI_AI");
        }
    }
}
