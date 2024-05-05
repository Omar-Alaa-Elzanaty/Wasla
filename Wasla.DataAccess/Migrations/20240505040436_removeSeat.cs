using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeSeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    setNum = table.Column<int>(type: "int", nullable: false),
                    TripTmeTableId = table.Column<int>(type: "int", nullable: false),
                    TripTimeTableId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => new { x.setNum, x.TripTmeTableId });
                    table.ForeignKey(
                        name: "FK_Seats_TripTimeTables_TripTimeTableId",
                        column: x => x.TripTimeTableId,
                        principalTable: "TripTimeTables",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TripTimeTableId",
                table: "Seats",
                column: "TripTimeTableId");
        }
    }
}
