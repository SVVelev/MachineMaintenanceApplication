using Microsoft.EntityFrameworkCore.Migrations;

namespace MachineMaintenanceApp.Data.Migrations
{
    public partial class SparePartUserUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SpareParts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpareParts_UserId",
                table: "SpareParts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpareParts_AspNetUsers_UserId",
                table: "SpareParts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpareParts_AspNetUsers_UserId",
                table: "SpareParts");

            migrationBuilder.DropIndex(
                name: "IX_SpareParts_UserId",
                table: "SpareParts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SpareParts");
        }
    }
}
