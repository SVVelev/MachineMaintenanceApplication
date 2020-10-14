using Microsoft.EntityFrameworkCore.Migrations;

namespace MachineMaintenanceApp.Data.Migrations
{
    public partial class UpdateSparePartsAndRepairs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SparePartId",
                table: "UnplannedRepairs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SparePartId",
                table: "PlannedRepairs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnplannedRepairs_SparePartId",
                table: "UnplannedRepairs",
                column: "SparePartId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedRepairs_SparePartId",
                table: "PlannedRepairs",
                column: "SparePartId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedRepairs_SpareParts_SparePartId",
                table: "PlannedRepairs",
                column: "SparePartId",
                principalTable: "SpareParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnplannedRepairs_SpareParts_SparePartId",
                table: "UnplannedRepairs",
                column: "SparePartId",
                principalTable: "SpareParts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlannedRepairs_SpareParts_SparePartId",
                table: "PlannedRepairs");

            migrationBuilder.DropForeignKey(
                name: "FK_UnplannedRepairs_SpareParts_SparePartId",
                table: "UnplannedRepairs");

            migrationBuilder.DropIndex(
                name: "IX_UnplannedRepairs_SparePartId",
                table: "UnplannedRepairs");

            migrationBuilder.DropIndex(
                name: "IX_PlannedRepairs_SparePartId",
                table: "PlannedRepairs");

            migrationBuilder.DropColumn(
                name: "SparePartId",
                table: "UnplannedRepairs");

            migrationBuilder.DropColumn(
                name: "SparePartId",
                table: "PlannedRepairs");
        }
    }
}
