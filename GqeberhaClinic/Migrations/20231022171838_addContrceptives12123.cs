using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GqeberhaClinic.Migrations
{
    public partial class addContrceptives12123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContraceptiveName",
                table: "ContraceptivePrescription",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ContraceptivePrescription",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ContraceptivePrescription");

            migrationBuilder.AlterColumn<string>(
                name: "ContraceptiveName",
                table: "ContraceptivePrescription",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
