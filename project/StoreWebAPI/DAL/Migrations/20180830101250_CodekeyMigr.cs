using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class CodekeyMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "CouponCode",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CouponCode_Code",
                table: "CouponCode",
                column: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CouponCode_Code",
                table: "CouponCode");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "CouponCode",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
