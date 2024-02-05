using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class database2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Users_Id",
                schema: "Account",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Trips_TripId",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Trips_TripId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Trips_TripId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Drivers_DriverId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Vehicles_VehicleId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_DriverId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_VehicleId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Seats_TripId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "From",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "To",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "LicenseImageUrl",
                schema: "Account",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LicenseNum",
                schema: "Account",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "NationalId",
                schema: "Account",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "AvailablePackageSpace",
                table: "Trips",
                newName: "AdsPrice");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Trips",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TripTimeTableId",
                table: "Seats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TripTimeTableId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Advertisments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Advertisments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartId = table.Column<int>(type: "int", nullable: false),
                    EndId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lines_Stations_EndId",
                        column: x => x.EndId,
                        principalTable: "Stations",
                        principalColumn: "StationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lines_Stations_StartId",
                        column: x => x.StartId,
                        principalTable: "Stations",
                        principalColumn: "StationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicDrivers",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LicenseImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicDrivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicDrivers_Users_Id",
                        column: x => x.Id,
                        principalSchema: "Account",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripTimeTables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    IsStart = table.Column<bool>(type: "bit", nullable: false),
                    AvailablePackageSpace = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripTimeTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripTimeTables_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Account",
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripTimeTables_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripTimeTables_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TripTimeTableId",
                table: "Seats",
                column: "TripTimeTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TripTimeTableId",
                table: "Reservations",
                column: "TripTimeTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisments_CustomerId",
                table: "Advertisments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_EndId",
                table: "Lines",
                column: "EndId");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_StartId",
                table: "Lines",
                column: "StartId");

            migrationBuilder.CreateIndex(
                name: "IX_TripTimeTables_DriverId",
                table: "TripTimeTables",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TripTimeTables_TripId",
                table: "TripTimeTables",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TripTimeTables_VehicleId",
                table: "TripTimeTables",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisments_Customer_CustomerId",
                table: "Advertisments",
                column: "CustomerId",
                principalSchema: "Account",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_PublicDrivers_Id",
                schema: "Account",
                table: "Drivers",
                column: "Id",
                principalSchema: "Account",
                principalTable: "PublicDrivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_TripTimeTables_TripId",
                table: "Packages",
                column: "TripId",
                principalTable: "TripTimeTables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_TripTimeTables_TripTimeTableId",
                table: "Reservations",
                column: "TripTimeTableId",
                principalTable: "TripTimeTables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Trips_TripId",
                table: "Reservations",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_TripTimeTables_TripTimeTableId",
                table: "Seats",
                column: "TripTimeTableId",
                principalTable: "TripTimeTables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisments_Customer_CustomerId",
                table: "Advertisments");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_PublicDrivers_Id",
                schema: "Account",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_TripTimeTables_TripId",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_TripTimeTables_TripTimeTableId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Trips_TripId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_TripTimeTables_TripTimeTableId",
                table: "Seats");

            migrationBuilder.DropTable(
                name: "Lines");

            migrationBuilder.DropTable(
                name: "PublicDrivers",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "TripTimeTables");

            migrationBuilder.DropIndex(
                name: "IX_Seats_TripTimeTableId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_TripTimeTableId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Advertisments_CustomerId",
                table: "Advertisments");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TripTimeTableId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "TripTimeTableId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Advertisments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Advertisments");

            migrationBuilder.RenameColumn(
                name: "AdsPrice",
                table: "Trips",
                newName: "AvailablePackageSpace");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DriverId",
                table: "Trips",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LicenseImageUrl",
                schema: "Account",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LicenseNum",
                schema: "Account",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                schema: "Account",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_DriverId",
                table: "Trips",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_VehicleId",
                table: "Trips",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TripId",
                table: "Seats",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Users_Id",
                schema: "Account",
                table: "Drivers",
                column: "Id",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Trips_TripId",
                table: "Packages",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Trips_TripId",
                table: "Reservations",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Trips_TripId",
                table: "Seats",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Drivers_DriverId",
                table: "Trips",
                column: "DriverId",
                principalSchema: "Account",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Vehicles_VehicleId",
                table: "Trips",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
