using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace bazargharnext.Migrations
{
    public partial class category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
        name: "product",
                columns: table => new
                {
                    Product_Id = table.Column<int>(nullable: false).Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Product_Name = table.Column<string>(nullable: false),
                   
                    Price = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: false),

                },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_product", x => x.Product_Id);
                       
                    });
            migrationBuilder.AddColumn<int>(
                name: "Category_id",
                table: "product",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_Category_id",
                table: "product",
                column: "Category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_product_category_Category_id",
                table: "product",
                column: "Category_id",
                principalTable: "category",
                principalColumn: "Category_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product");
            migrationBuilder.DropForeignKey(
                name: "FK_product_category_Category_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_Category_id",
                table: "product");

            migrationBuilder.DropColumn(
                name: "Category_id",
                table: "product");
        }
    }
}
