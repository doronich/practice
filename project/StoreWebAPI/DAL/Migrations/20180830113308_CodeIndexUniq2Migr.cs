using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class CodeIndexUniq2Migr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponCode_Users_UserId",
                table: "CouponCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CouponCode_CodeId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CouponCode",
                table: "CouponCode");

            migrationBuilder.RenameTable(
                name: "CouponCode",
                newName: "CouponCodes");

            migrationBuilder.RenameIndex(
                name: "IX_CouponCode_UserId",
                table: "CouponCodes",
                newName: "IX_CouponCodes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CouponCode_Code",
                table: "CouponCodes",
                newName: "IX_CouponCodes_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CouponCodes",
                table: "CouponCodes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CouponCodes_Users_UserId",
                table: "CouponCodes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CouponCodes_CodeId",
                table: "Orders",
                column: "CodeId",
                principalTable: "CouponCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponCodes_Users_UserId",
                table: "CouponCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CouponCodes_CodeId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CouponCodes",
                table: "CouponCodes");

            migrationBuilder.RenameTable(
                name: "CouponCodes",
                newName: "CouponCode");

            migrationBuilder.RenameIndex(
                name: "IX_CouponCodes_UserId",
                table: "CouponCode",
                newName: "IX_CouponCode_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CouponCodes_Code",
                table: "CouponCode",
                newName: "IX_CouponCode_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CouponCode",
                table: "CouponCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CouponCode_Users_UserId",
                table: "CouponCode",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CouponCode_CodeId",
                table: "Orders",
                column: "CodeId",
                principalTable: "CouponCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
