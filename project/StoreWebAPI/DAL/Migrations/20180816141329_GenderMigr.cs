using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class GenderMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "Sex",
                table: "Items",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "Sex",
                table: "Items",
                nullable: false,
                defaultValue: 0);
        }
    }
}
