using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamBee.Migrations
{
    public partial class AddGhiChuOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "Order");
        }
    }
}
