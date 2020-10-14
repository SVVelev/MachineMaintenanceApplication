using Microsoft.EntityFrameworkCore.Migrations;

namespace MachineMaintenanceApp.Data.Migrations
{
    public partial class RepairsUpdateDatabase1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SparePartNumber",
                table: "UnplannedRepairs");

            migrationBuilder.DropColumn(
                name: "SparePartNumber",
                table: "PlannedRepairs");

            migrationBuilder.AddColumn<string>(
                name: "PartNumber",
                table: "UnplannedRepairs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartNumber",
                table: "PlannedRepairs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartNumber",
                table: "UnplannedRepairs");

            migrationBuilder.DropColumn(
                name: "PartNumber",
                table: "PlannedRepairs");

            migrationBuilder.AddColumn<string>(
                name: "SparePartNumber",
                table: "UnplannedRepairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SparePartNumber",
                table: "PlannedRepairs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
