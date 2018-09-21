using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class DiscountCodeMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CodeId",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Orders",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "Items",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "CouponCode",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    Discount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponCode_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CodeId",
                table: "Orders",
                column: "CodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponCode_UserId",
                table: "CouponCode",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CouponCode_CodeId",
                table: "Orders",
                column: "CodeId",
                principalTable: "CouponCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CouponCode_CodeId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "CouponCode");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CodeId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CodeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "Discount",
                table: "Items",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
