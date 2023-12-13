using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editvehicleorg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Organizations_OrganizationId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_OrganizationId",
                table: "Vehicles");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_OrganizationId",
                table: "Vehicles",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Organizations_OrganizationId",
                table: "Vehicles",
                column: "OrganizationId",
                principalSchema: "Account",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Organizations_OrganizationId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_OrganizationId",
                table: "Vehicles");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_OrganizationId",
                table: "Vehicles",
                column: "OrganizationId",
                unique: true,
                filter: "[OrganizationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Organizations_OrganizationId",
                table: "Vehicles",
                column: "OrganizationId",
                principalSchema: "Account",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
