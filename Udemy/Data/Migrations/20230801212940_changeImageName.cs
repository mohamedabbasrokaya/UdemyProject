using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Udemy.Data.Migrations
{
    public partial class changeImageName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Categories",
                newName: "ImageName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Categories",
                newName: "ImagePath");
        }
    }
}
