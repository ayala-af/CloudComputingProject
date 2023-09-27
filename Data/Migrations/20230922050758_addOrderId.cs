using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudComputingProject.Data.Migrations
{
    public partial class addOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Products",
                newName: "Category");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Products",
                newName: "Category");
        }
    }
}
