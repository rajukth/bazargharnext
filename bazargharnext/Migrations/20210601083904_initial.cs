using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bazargharnext.Migrations
{
    public partial class initial : Migration
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
                name: "product",
                columns: table => new
                {
                    Product_Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Userid = table.Column<int>(nullable: false),
                    Product_name = table.Column<string>(nullable: true),
                    Category_id = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Category_id1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.Product_Id);
                    table.ForeignKey(
                        name: "FK_product_category_Category_id1",
                        column: x => x.Category_id1,
                        principalTable: "category",
                        principalColumn: "Category_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Comment_Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Product_Id = table.Column<int>(nullable: false),
                    Comment_Text = table.Column<string>(nullable: true),
                    User_By = table.Column<int>(nullable: false),
                    Reply_To = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    React = table.Column<bool>(nullable: false),
                    Product_Id1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Comment_Id);
                    table.ForeignKey(
                        name: "FK_Comments_product_Product_Id1",
                        column: x => x.Product_Id1,
                        principalTable: "product",
                        principalColumn: "Product_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Product_Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true),
                    Product_Id1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gallery_product_Product_Id1",
                        column: x => x.Product_Id1,
                        principalTable: "product",
                        principalColumn: "Product_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "product_details",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Product_Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Product_Id1 = table.Column<int>(nullable: true)
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
                name: "IX_Comments_Product_Id1",
                table: "Comments",
                column: "Product_Id1");

            migrationBuilder.CreateIndex(
                name: "IX_Gallery_Product_Id1",
                table: "Gallery",
                column: "Product_Id1");

            migrationBuilder.CreateIndex(
                name: "IX_product_Category_id1",
                table: "product",
                column: "Category_id1");

            migrationBuilder.CreateIndex(
                name: "IX_product_details_Product_Id1",
                table: "product_details",
                column: "Product_Id1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Gallery");

            migrationBuilder.DropTable(
                name: "product_details");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "category");
        }
    }
}
