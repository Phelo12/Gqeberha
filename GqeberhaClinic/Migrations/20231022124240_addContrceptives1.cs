using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GqeberhaClinic.Migrations
{
    public partial class addContrceptives1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EfficacyRate",
                table: "Contraceptive",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "EfficacyRate",
                table: "Contraceptive",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000,
                oldNullable: true);
        }
    }
}
