using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Data.Migrations
{
    public partial class userid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "UserInfos");

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "UserInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "UserInfos");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "UserInfos",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
