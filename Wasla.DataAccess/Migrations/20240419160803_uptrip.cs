using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class uptrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "BreakPeriod",
                table: "TripTimeTables",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "IsRide",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OnRoad",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakPeriod",
                table: "TripTimeTables");

            migrationBuilder.DropColumn(
                name: "IsRide",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "OnRoad",
                table: "Reservations");
        }
    }
}
