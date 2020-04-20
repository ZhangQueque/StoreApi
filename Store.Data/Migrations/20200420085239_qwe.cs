using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Data.Migrations
{
    public partial class qwe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    UserEmail = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NickName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    IsExistPhone = table.Column<int>(nullable: false),
                    IsExistEmail = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateId = table.Column<int>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "User_Roles");

            migrationBuilder.DropTable(
                name: "UserInfos");
        }
    }
}
