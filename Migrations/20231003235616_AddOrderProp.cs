using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudComputingProject.Migrations
{
    public partial class AddOrderProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Order",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "ClientName",
                table: "Order",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "OrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "House",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ClientFirstName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientLastName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "FeelsLike",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Humidity",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsHoliday",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrderDay",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Temperature",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ClientFirstName",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ClientLastName",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "FeelsLike",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Humidity",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IsHoliday",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderDay",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Order",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Order",
                newName: "ClientName");

            migrationBuilder.AlterColumn<string>(
                name: "House",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
