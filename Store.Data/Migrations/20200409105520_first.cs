using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Data.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 20, nullable: true),
                    PId = table.Column<int>(nullable: false),
                    SortId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product_Sizes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: true),
                    Product_CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Sizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Sizes_product_Categories_Product_CategoryId",
                        column: x => x.Product_CategoryId,
                        principalTable: "product_Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    ShortDescribe = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    Pictrue = table.Column<string>(nullable: true),
                    ProductNo = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Stock = table.Column<int>(nullable: false),
                    PageView = table.Column<int>(nullable: false),
                    Purchase = table.Column<int>(nullable: false),
                    Product_CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_product_Categories_Product_CategoryId",
                        column: x => x.Product_CategoryId,
                        principalTable: "product_Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Describes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Describes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Describes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Images",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Image = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Images_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_Describes_ProductId",
                table: "Product_Describes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Images_ProductId",
                table: "Product_Images",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Sizes_Product_CategoryId",
                table: "Product_Sizes",
                column: "Product_CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Product_CategoryId",
                table: "Products",
                column: "Product_CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product_Describes");

            migrationBuilder.DropTable(
                name: "Product_Images");

            migrationBuilder.DropTable(
                name: "Product_Sizes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "product_Categories");
        }
    }
}
