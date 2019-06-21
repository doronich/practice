using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class CategoryMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Subkind",
                table: "Items");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Kind",
                table: "Items",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Subkind",
                table: "Items",
                nullable: true);
        }
    }
}
