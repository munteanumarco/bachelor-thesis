using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class AddEventType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "EmergencyEvents");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "EmergencyEvents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "EmergencyEvents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "EmergencyEvents");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "EmergencyEvents",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "EmergencyEvents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
