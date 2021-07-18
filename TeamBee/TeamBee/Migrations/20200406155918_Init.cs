using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamBee.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateModify = table.Column<DateTime>(nullable: false),
                    URL = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    NameFirst = table.Column<string>(maxLength: 100, nullable: false),
                    NameLast = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Permission = table.Column<int>(nullable: false),
                    Active = table.Column<int>(nullable: false),
                    Confirm = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category_Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    Price = table.Column<int>(nullable: false),
                    SalePrice = table.Column<int>(nullable: false),
                    Thumbnail = table.Column<string>(nullable: true),
                    Stars = table.Column<double>(nullable: false),
                    Views = table.Column<int>(nullable: false),
                    Orders = table.Column<int>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateModify = table.Column<DateTime>(nullable: false),
                    URL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_Category_Id",
                        column: x => x.Category_Id,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    User_Id = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<int>(nullable: false),
                    TotalQuantity = table.Column<int>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    ShipEmail = table.Column<string>(nullable: true),
                    ShipPhone = table.Column<string>(maxLength: 20, nullable: false),
                    ShipName = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Pay = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoOrder_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    User_Id = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<int>(nullable: false),
                    TotalQuantity = table.Column<int>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    ShipEmail = table.Column<string>(nullable: true),
                    ShipPhone = table.Column<string>(maxLength: 20, nullable: false),
                    ShipName = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Pay = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    User_Id = table.Column<int>(nullable: false),
                    Product_Id = table.Column<int>(nullable: false),
                    Stars = table.Column<double>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateModify = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Product_Product_Id",
                        column: x => x.Product_Id,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_User_User_Id",
                        column: x => x.User_Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cart_Id = table.Column<int>(nullable: false),
                    Product_Id = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    PriceSingle = table.Column<int>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartDetail_Order_Cart_Id",
                        column: x => x.Cart_Id,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartDetail_Product_Product_Id",
                        column: x => x.Product_Id,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoCartDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cart_Id = table.Column<int>(nullable: false),
                    Product_Id = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    PriceSingle = table.Column<int>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    NoOrderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoCartDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoCartDetail_Order_Cart_Id",
                        column: x => x.Cart_Id,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoCartDetail_NoOrder_NoOrderId",
                        column: x => x.NoOrderId,
                        principalTable: "NoOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NoCartDetail_Product_Product_Id",
                        column: x => x.Product_Id,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartDetail_Cart_Id",
                table: "CartDetail",
                column: "Cart_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetail_Product_Id",
                table: "CartDetail",
                column: "Product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                table: "Category",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Product_Id",
                table: "Comment",
                column: "Product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_User_Id",
                table: "Comment",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_NoCartDetail_Cart_Id",
                table: "NoCartDetail",
                column: "Cart_Id");

            migrationBuilder.CreateIndex(
                name: "IX_NoCartDetail_NoOrderId",
                table: "NoCartDetail",
                column: "NoOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_NoCartDetail_Product_Id",
                table: "NoCartDetail",
                column: "Product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_NoOrder_User_Id",
                table: "NoOrder",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_User_Id",
                table: "Order",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Category_Id",
                table: "Product",
                column: "Category_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_PhoneNumber",
                table: "User",
                column: "PhoneNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartDetail");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "NoCartDetail");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "NoOrder");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
