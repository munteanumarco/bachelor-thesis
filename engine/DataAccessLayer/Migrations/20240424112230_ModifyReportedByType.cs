using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class ModifyReportedByType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportedBy",
                table: "EmergencyEvents");

            migrationBuilder.AddColumn<Guid>(
                name: "ReportedBy",
                table: "EmergencyEvents",
                type: "uuid",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportedBy",
                table: "EmergencyEvents");

            migrationBuilder.AddColumn<int>(
                name: "ReportedBy",
                table: "EmergencyEvents",
                type: "integer",
                nullable: true);
        }
    }
}
