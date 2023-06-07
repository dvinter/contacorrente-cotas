using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class ajuste_dados_banco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM [fluxo_conta]    WHERE Tipo = 400;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
