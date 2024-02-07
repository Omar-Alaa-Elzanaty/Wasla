using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class driver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_PublicDrivers_Id",
                schema: "Account",
                table: "Drivers");

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

            migrationBuilder.CreateTable(
                name: "PublicDriversRates",
                columns: table => new
                {
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rate = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicDriversRates", x => new { x.DriverId, x.CustomerId });
                    table.ForeignKey(
                        name: "FK_PublicDriversRates_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Account",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicDriversRates_PublicDrivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "Account",
                        principalTable: "PublicDrivers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicDriversRates_CustomerId",
                table: "PublicDriversRates",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Users_Id",
                schema: "Account",
                table: "Drivers",
                column: "Id",
                principalSchema: "Account",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Users_Id",
                schema: "Account",
                table: "Drivers");

            migrationBuilder.DropTable(
                name: "PublicDriversRates");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_PublicDrivers_Id",
                schema: "Account",
                table: "Drivers",
                column: "Id",
                principalSchema: "Account",
                principalTable: "PublicDrivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
