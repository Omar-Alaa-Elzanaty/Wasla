using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateTripTimeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripTimeTables_Drivers_DriverId",
                table: "TripTimeTables");

            migrationBuilder.AddForeignKey(
                name: "FK_TripTimeTables_PublicDrivers_DriverId",
                table: "TripTimeTables",
                column: "DriverId",
                principalSchema: "Account",
                principalTable: "PublicDrivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripTimeTables_PublicDrivers_DriverId",
                table: "TripTimeTables");

            migrationBuilder.AddForeignKey(
                name: "FK_TripTimeTables_Drivers_DriverId",
                table: "TripTimeTables",
                column: "DriverId",
                principalSchema: "Account",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
