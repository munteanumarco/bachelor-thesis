using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class AddImagesColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<string>(
                name: "ProcessedImage",
                table: "LandCoverAnalyses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RawImage",
                table: "LandCoverAnalyses",
                type: "text",
                nullable: true);
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedImage",
                table: "LandCoverAnalyses");

            migrationBuilder.DropColumn(
                name: "RawImage",
                table: "LandCoverAnalyses");
            
        }
    }
}
