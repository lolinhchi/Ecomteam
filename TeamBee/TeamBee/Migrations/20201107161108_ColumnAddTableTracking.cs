using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamBee.Migrations
{
    public partial class ColumnAddTableTracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "Tracking",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTen",
                table: "Tracking",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SDT",
                table: "Tracking",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TongTien",
                table: "Tracking",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "Tracking");

            migrationBuilder.DropColumn(
                name: "HoTen",
                table: "Tracking");

            migrationBuilder.DropColumn(
                name: "SDT",
                table: "Tracking");

            migrationBuilder.DropColumn(
                name: "TongTien",
                table: "Tracking");
        }
    }
}
