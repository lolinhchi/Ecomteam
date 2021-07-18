using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamBee.Migrations
{
    public partial class AddTable_Blog_BlogItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blog_Category",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateModify = table.Column<DateTime>(nullable: false),
                    URL = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category_BlogId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Thumbnail = table.Column<string>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    URL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogItem_Blog_Category_Category_BlogId",
                        column: x => x.Category_BlogId,
                        principalTable: "Blog_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogItem_Category_BlogId",
                table: "BlogItem",
                column: "Category_BlogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogItem");

            migrationBuilder.DropTable(
                name: "Blog_Category");
        }
    }
}
