using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class CodeIndexUniqMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CouponCode_Code",
                table: "CouponCode");

            migrationBuilder.CreateIndex(
                name: "IX_CouponCode_Code",
                table: "CouponCode",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CouponCode_Code",
                table: "CouponCode");

            migrationBuilder.CreateIndex(
                name: "IX_CouponCode_Code",
                table: "CouponCode",
                column: "Code");
        }
    }
}
