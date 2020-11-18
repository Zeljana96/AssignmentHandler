using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks_Handler.Migrations
{
    public partial class TimeSpentAndWayOfSolving : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeSpent",
                table: "Assignments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WayOfSolving",
                table: "Assignments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSpent",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "WayOfSolving",
                table: "Assignments");
        }
    }
}
