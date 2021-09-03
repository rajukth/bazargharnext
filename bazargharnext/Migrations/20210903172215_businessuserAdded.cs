using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bazargharnext.Migrations
{
    public partial class businessuserAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "BusinessUser",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    User_Id = table.Column<int>(nullable: false),
                    Office_No = table.Column<int>(nullable: false),
                    RegistrationNo = table.Column<string>(nullable: true),
                    PanNo = table.Column<string>(nullable: true),
                    Registration_Image = table.Column<string>(nullable: true),
                    Pan_Image = table.Column<string>(nullable: true),
                    About = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessUser", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessUserRequest",
                columns: table => new
                {
                    BusinessRegId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Userid = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Contact = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    UserRole = table.Column<string>(nullable: true),
                    Office_No = table.Column<int>(nullable: false),
                    RegistrationNo = table.Column<string>(nullable: true),
                    PanNo = table.Column<string>(nullable: true),
                    Registration_Image = table.Column<string>(nullable: true),
                    Pan_Image = table.Column<string>(nullable: true),
                    About = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessUserRequest", x => x.BusinessRegId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessUser");

            migrationBuilder.DropTable(
                name: "BusinessUserRequest");

           
        }
    }
}
