using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class stationPublic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicDriverTrips_Stations_EndStationId",
                table: "PublicDriverTrips");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicDriverTrips_Stations_StartStationId",
                table: "PublicDriverTrips");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicDriverTrips_PublicStations_EndStationId",
                table: "PublicDriverTrips",
                column: "EndStationId",
                principalTable: "PublicStations",
                principalColumn: "StationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicDriverTrips_PublicStations_StartStationId",
                table: "PublicDriverTrips",
                column: "StartStationId",
                principalTable: "PublicStations",
                principalColumn: "StationId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicDriverTrips_PublicStations_EndStationId",
                table: "PublicDriverTrips");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicDriverTrips_PublicStations_StartStationId",
                table: "PublicDriverTrips");

            migrationBuilder.AddForeignKey(
                name: "FK_PublicDriverTrips_Stations_EndStationId",
                table: "PublicDriverTrips",
                column: "EndStationId",
                principalTable: "Stations",
                principalColumn: "StationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicDriverTrips_Stations_StartStationId",
                table: "PublicDriverTrips",
                column: "StartStationId",
                principalTable: "Stations",
                principalColumn: "StationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
