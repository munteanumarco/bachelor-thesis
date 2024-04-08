using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class AddEmergencyEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61a752af-6cb6-4ecc-a1de-ba06c101eed8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3b42aef-fb94-47ef-8600-54d8d4782b98");

            migrationBuilder.CreateTable(
                name: "EmergencyEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReportedBy = table.Column<int>(type: "integer", nullable: true),
                    ReportedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyEvents", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "56d4a6fb-f6fb-4e93-aa52-c3e2b737746f", "2a6a5574-0957-4e9a-8daf-1ffe106b2186", "User", "USER" },
                    { "6d05d079-5e04-4e5a-b604-a43657fc9825", "0a5ea81f-7eea-44ea-bce5-1e9cf3c4f02d", "Admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmergencyEvents");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56d4a6fb-f6fb-4e93-aa52-c3e2b737746f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d05d079-5e04-4e5a-b604-a43657fc9825");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "61a752af-6cb6-4ecc-a1de-ba06c101eed8", "13c1750e-35c9-41d8-995d-f97276bc1234", "User", "USER" },
                    { "a3b42aef-fb94-47ef-8600-54d8d4782b98", "c5b64766-8ccd-485c-8a4e-c40519f2299c", "Admin", "ADMIN" }
                });
        }
    }
}
