using Microsoft.EntityFrameworkCore.Migrations;

namespace WolontariuszPlus.Data.Migrations
{
    public partial class AddedImageRelativePathToEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageRelativePath",
                table: "Events",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageRelativePath",
                table: "Events");
        }
    }
}
