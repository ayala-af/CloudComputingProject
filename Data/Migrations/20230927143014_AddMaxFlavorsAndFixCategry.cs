using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudComputingProject.Data.Migrations
{
    public partial class AddMaxFlavorsAndFixCategry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxFlavorsNumber",
                table: "Products",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxFlavorsNumber",
                table: "Products");
        }
    }
}
