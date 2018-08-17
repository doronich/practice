using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DL.Migrations
{
    public partial class ImagesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Items",
                newName: "PreviewImagePath");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath1",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath2",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath3",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ImagePath2",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ImagePath3",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "PreviewImagePath",
                table: "Items",
                newName: "ImagePath");
        }
    }
}
