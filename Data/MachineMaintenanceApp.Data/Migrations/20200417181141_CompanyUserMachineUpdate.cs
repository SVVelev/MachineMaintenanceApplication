using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MachineMaintenanceApp.Data.Migrations
{
    public partial class CompanyUserMachineUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ManufactureYear",
                table: "Machines",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Machines",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpareParts_InventoryNumber",
                table: "SpareParts",
                column: "InventoryNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Machines_ApplicationUserId",
                table: "Machines",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_InventoryNumber",
                table: "Machines",
                column: "InventoryNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CardNumber",
                table: "AspNetUsers",
                column: "CardNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_IsDeleted",
                table: "Company",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Name",
                table: "Company",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Company_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_AspNetUsers_ApplicationUserId",
                table: "Machines",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Company_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Machines_AspNetUsers_ApplicationUserId",
                table: "Machines");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropIndex(
                name: "IX_SpareParts_InventoryNumber",
                table: "SpareParts");

            migrationBuilder.DropIndex(
                name: "IX_Machines_ApplicationUserId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_InventoryNumber",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CardNumber",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ManufactureYear",
                table: "Machines",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
