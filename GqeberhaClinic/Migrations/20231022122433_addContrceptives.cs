using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GqeberhaClinic.Migrations
{
    public partial class addContrceptives : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contraceptive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationOfEffectiveness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MethodOfUse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EfficacyRate = table.Column<double>(type: "float", nullable: false),
                    SideEffects = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Benefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Contraindications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reversibility = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contraceptive", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contraceptive");
        }
    }
}
