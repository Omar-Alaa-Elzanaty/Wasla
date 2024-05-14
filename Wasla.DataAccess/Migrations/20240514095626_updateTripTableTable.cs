using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateTripTableTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripTimeTables_PublicDrivers_DriverId",
                table: "TripTimeTables");

            migrationBuilder.AddColumn<string>(
                name: "PublicDriverId",
                table: "TripTimeTables",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripTimeTables_PublicDriverId",
                table: "TripTimeTables",
                column: "PublicDriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripTimeTables_Drivers_DriverId",
                table: "TripTimeTables",
                column: "DriverId",
                principalSchema: "Account",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TripTimeTables_PublicDrivers_PublicDriverId",
                table: "TripTimeTables",
                column: "PublicDriverId",
                principalSchema: "Account",
                principalTable: "PublicDrivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripTimeTables_Drivers_DriverId",
                table: "TripTimeTables");

            migrationBuilder.DropForeignKey(
                name: "FK_TripTimeTables_PublicDrivers_PublicDriverId",
                table: "TripTimeTables");

            migrationBuilder.DropIndex(
                name: "IX_TripTimeTables_PublicDriverId",
                table: "TripTimeTables");

            migrationBuilder.DropColumn(
                name: "PublicDriverId",
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
    }
}
