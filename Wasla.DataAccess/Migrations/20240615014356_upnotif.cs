using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class upnotif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotifactionImage",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NotifactionName",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifactionImage",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NotifactionName",
                table: "Notifications");
        }
    }
}
