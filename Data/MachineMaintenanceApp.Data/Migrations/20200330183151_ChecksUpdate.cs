using Microsoft.EntityFrameworkCore.Migrations;

namespace MachineMaintenanceApp.Data.Migrations
{
    public partial class ChecksUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "WeeklyChecks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "DailyChecks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "WeeklyChecks");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "DailyChecks");
        }
    }
}
