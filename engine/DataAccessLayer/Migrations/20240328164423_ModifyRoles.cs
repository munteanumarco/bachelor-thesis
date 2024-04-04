using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class ModifyRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ba8e759-c128-44c1-94ab-67af50c095ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e91d7920-c16d-4340-8693-4b545df14fb9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "61a752af-6cb6-4ecc-a1de-ba06c101eed8", "13c1750e-35c9-41d8-995d-f97276bc1234", "User", "USER" },
                    { "a3b42aef-fb94-47ef-8600-54d8d4782b98", "c5b64766-8ccd-485c-8a4e-c40519f2299c", "Admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61a752af-6cb6-4ecc-a1de-ba06c101eed8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3b42aef-fb94-47ef-8600-54d8d4782b98");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1ba8e759-c128-44c1-94ab-67af50c095ae", "88366ca3-f452-4525-8d16-1ece38835e6e", "Admin", null },
                    { "e91d7920-c16d-4340-8693-4b545df14fb9", "7c3e93d2-e1c7-4183-a3dd-55d10ca5dc16", "User", null }
                });
        }
    }
}
