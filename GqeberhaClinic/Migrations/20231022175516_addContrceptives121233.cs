using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GqeberhaClinic.Migrations
{
    public partial class addContrceptives121233 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Referrals",
                columns: table => new
                {
                    ReferralId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NurseID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReferralDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReasonForReferral = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferredTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferringDoctor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreventionID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referrals", x => x.ReferralId);
                    table.ForeignKey(
                        name: "FK_Referrals_AspNetUsers_NurseID",
                        column: x => x.NurseID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Referrals_ContraceptivePrescription_PreventionID",
                        column: x => x.PreventionID,
                        principalTable: "ContraceptivePrescription",
                        principalColumn: "PrescriptionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_NurseID",
                table: "Referrals",
                column: "NurseID");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_PreventionID",
                table: "Referrals",
                column: "PreventionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Referrals");
        }
    }
}
