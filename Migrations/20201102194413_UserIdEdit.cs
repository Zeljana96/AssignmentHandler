using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks_Handler.Migrations
{
    public partial class UserIdEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserIdEdit",
                table: "Assignments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIdEdit",
                table: "Assignments");
        }
    }
}
