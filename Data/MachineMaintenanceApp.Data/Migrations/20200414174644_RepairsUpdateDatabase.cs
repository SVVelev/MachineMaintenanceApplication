using Microsoft.EntityFrameworkCore.Migrations;

namespace MachineMaintenanceApp.Data.Migrations
{
    public partial class RepairsUpdateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SparePartNumber",
                table: "UnplannedRepairs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SparePartNumber",
                table: "PlannedRepairs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SparePartNumber",
                table: "UnplannedRepairs");

            migrationBuilder.DropColumn(
                name: "SparePartNumber",
                table: "PlannedRepairs");
        }
    }
}
