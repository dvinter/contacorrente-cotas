using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class logs_change_refid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_Entity_Key",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Logs");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Logs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "RefId",
                table: "Logs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Entity_RefId",
                table: "Logs",
                columns: new[] { "Entity", "RefId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_Entity_RefId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "RefId",
                table: "Logs");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Logs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Entity_Key",
                table: "Logs",
                columns: new[] { "Entity", "Key" });
        }
    }
}
