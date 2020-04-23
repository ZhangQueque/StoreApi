using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Data.Migrations
{
    public partial class realprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RealPrice",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Createtime",
                table: "Wishes",
                newName: "CreateTime");

            migrationBuilder.RenameColumn(
                name: "Createtime",
                table: "Orders",
                newName: "CreateTime");

            migrationBuilder.RenameColumn(
                name: "Createtime",
                table: "Carts",
                newName: "CreateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "Wishes",
                newName: "Createtime");

            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "Orders",
                newName: "Createtime");

            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "Carts",
                newName: "Createtime");

            migrationBuilder.AddColumn<int>(
                name: "RealPrice",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
