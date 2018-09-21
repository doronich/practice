using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class MiniImageMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MinPreviewImagePath",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinPreviewImagePath",
                table: "Items");
        }
    }
}
