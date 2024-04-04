using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1ba8e759-c128-44c1-94ab-67af50c095ae", "88366ca3-f452-4525-8d16-1ece38835e6e", "Admin", null },
                    { "e91d7920-c16d-4340-8693-4b545df14fb9", "7c3e93d2-e1c7-4183-a3dd-55d10ca5dc16", "User", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ba8e759-c128-44c1-94ab-67af50c095ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e91d7920-c16d-4340-8693-4b545df14fb9");
        }
    }
}
