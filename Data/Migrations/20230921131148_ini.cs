using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudComputingProject.Data.Migrations
{
    public partial class ini : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlavorId1",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "FlavorId2",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "FlavorId3",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "FlavorId4",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "FlavorId5",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "Flavors",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flavors",
                table: "OrderItems");

            migrationBuilder.AddColumn<int>(
                name: "FlavorId1",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlavorId2",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlavorId3",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlavorId4",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlavorId5",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
