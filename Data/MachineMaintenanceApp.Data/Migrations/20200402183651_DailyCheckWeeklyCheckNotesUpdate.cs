using Microsoft.EntityFrameworkCore.Migrations;

namespace MachineMaintenanceApp.Data.Migrations
{
    public partial class DailyCheckWeeklyCheckNotesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "WeeklyChecks",
                type: "Text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "DailyChecks",
                type: "Text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "WeeklyChecks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "DailyChecks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Text",
                oldNullable: true);
        }
    }
}
