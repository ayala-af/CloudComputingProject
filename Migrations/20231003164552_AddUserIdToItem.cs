using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudComputingProject.Migrations
{
    public partial class AddUserIdToItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OrderItems");
        }
    }
}
