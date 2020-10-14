using Microsoft.EntityFrameworkCore.Migrations;

namespace MachineMaintenanceApp.Data.Migrations
{
    public partial class CompanyUpdateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Company",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Company");
        }
    }
}
