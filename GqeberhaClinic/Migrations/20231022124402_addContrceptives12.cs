using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GqeberhaClinic.Migrations
{
    public partial class addContrceptives12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cost",
                table: "Contraceptive",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldMaxLength: 5000);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Contraceptive",
                type: "decimal(18,2)",
                maxLength: 5000,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);
        }
    }
}
