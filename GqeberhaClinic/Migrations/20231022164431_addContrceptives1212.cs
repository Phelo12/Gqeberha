using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GqeberhaClinic.Migrations
{
    public partial class addContrceptives1212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContraceptivePrescription",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ContraceptiveName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrescribedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContraceptivePrescription", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_ContraceptivePrescription_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContraceptivePrescription_PatientId",
                table: "ContraceptivePrescription",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContraceptivePrescription");
        }
    }
}
