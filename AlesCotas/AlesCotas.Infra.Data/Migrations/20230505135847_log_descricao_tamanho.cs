using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class log_descricao_tamanho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Logs",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: false,
                collation: "SQL_LATIN1_GENERAL_CP1_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldCollation: "SQL_LATIN1_GENERAL_CP1_CI_AI");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Logs",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                collation: "SQL_LATIN1_GENERAL_CP1_CI_AI",
                oldClrType: typeof(string),
                oldType: "nvarchar(800)",
                oldMaxLength: 800,
                oldCollation: "SQL_LATIN1_GENERAL_CP1_CI_AI");
        }
    }
}
