using Microsoft.EntityFrameworkCore.Migrations;

namespace MachineMaintenanceApp.Data.Migrations
{
    public partial class UserMachineUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_AspNetUsers_ApplicationUserId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_ApplicationUserId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Machines");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Machines",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_UserId",
                table: "Machines",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_AspNetUsers_UserId",
                table: "Machines",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_AspNetUsers_UserId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_UserId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Machines");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Machines",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_ApplicationUserId",
                table: "Machines",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_AspNetUsers_ApplicationUserId",
                table: "Machines",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
