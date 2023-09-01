using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GqeberhaClinic.Migrations
{
    public partial class add2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Ques_AspNetUsers_ClinicianID",
                table: "Appointments_Ques");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_Ques_ClinicianID",
                table: "Appointments_Ques");

            migrationBuilder.DropColumn(
                name: "ClinicianID",
                table: "Appointments_Ques");

            migrationBuilder.AddColumn<string>(
                name: "Clinician",
                table: "Appointments_Ques",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Clinician",
                table: "Appointments_Ques");

            migrationBuilder.AddColumn<string>(
                name: "ClinicianID",
                table: "Appointments_Ques",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Ques_ClinicianID",
                table: "Appointments_Ques",
                column: "ClinicianID");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Ques_AspNetUsers_ClinicianID",
                table: "Appointments_Ques",
                column: "ClinicianID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
