using Microsoft.EntityFrameworkCore.Migrations;

namespace bazargharnext.Migrations
{
    public partial class deleteUseridfrombusiness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Userid",
                table: "BusinessUserRequest");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Userid",
                table: "BusinessUserRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
