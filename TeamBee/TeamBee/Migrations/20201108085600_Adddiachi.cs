using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamBee.Migrations
{
    public partial class Adddiachi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChi",
                table: "Tracking",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaChi",
                table: "Tracking");
        }
    }
}
