using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class CodeTypeMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Discount",
                table: "CouponCodes",
                nullable: false,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "CouponCodes",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
