using Microsoft.EntityFrameworkCore.Migrations;

namespace WolontariuszPlus.Data.Migrations
{
    public partial class RemovedRequiredConstraintFromRelativePathInEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageRelativePath",
                table: "Events",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageRelativePath",
                table: "Events",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
