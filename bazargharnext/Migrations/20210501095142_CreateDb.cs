using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bazargharnext.Migrations
{
    public partial class CreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    Category_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category_name = table.Column<string>(nullable: true),
                    Category_description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.Category_id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Customerid = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Customerid);
                });

            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    Product_Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Userid = table.Column<int>(nullable: false),
                    Product_name = table.Column<string>(nullable: true),
                    
                    Date = table.Column<DateTime>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.Product_Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Userid = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Userid);
                });

            migrationBuilder.CreateTable(
                name: "product_details",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Product_Id = table.Column<int>(nullable: false),
                    Product_Id1 = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_details_product_Product_Id1",
                        column: x => x.Product_Id1,
                        principalTable: "product",
                        principalColumn: "Product_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_details_Product_Id1",
                table: "product_details",
                column: "Product_Id1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Gallery");

            migrationBuilder.DropTable(
                name: "product_details");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "product");
        }
    }
}
