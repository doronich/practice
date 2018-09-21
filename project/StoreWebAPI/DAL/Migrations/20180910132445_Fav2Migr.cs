using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class Fav2Migr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteItems_Items_ItemId",
                table: "FavoriteItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteItems_Users_UserId",
                table: "FavoriteItems");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "FavoriteItems",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ItemId",
                table: "FavoriteItems",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteItems_Items_ItemId",
                table: "FavoriteItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteItems_Users_UserId",
                table: "FavoriteItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteItems_Items_ItemId",
                table: "FavoriteItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteItems_Users_UserId",
                table: "FavoriteItems");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "FavoriteItems",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "ItemId",
                table: "FavoriteItems",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteItems_Items_ItemId",
                table: "FavoriteItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteItems_Users_UserId",
                table: "FavoriteItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
