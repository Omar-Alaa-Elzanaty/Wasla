using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Addtrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LineId",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_LineId",
                table: "Trips",
                column: "LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Lines_LineId",
                table: "Trips",
                column: "LineId",
                principalTable: "Lines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Lines_LineId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_LineId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "Trips");
        }
    }
}
