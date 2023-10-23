using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GqeberhaClinic.Migrations
{
    public partial class addContrceptives121 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "FamilyPlanning_Screening",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "FamilyPlanning_Screening");
        }
    }
}
