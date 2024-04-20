using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class upfollowr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowRequests",
                table: "FollowRequests");

            migrationBuilder.AddColumn<int>(
                name: "FollowId",
                table: "FollowRequests",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowRequests",
                table: "FollowRequests",
                column: "FollowId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowRequests_SenderId",
                table: "FollowRequests",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowRequests",
                table: "FollowRequests");

            migrationBuilder.DropIndex(
                name: "IX_FollowRequests_SenderId",
                table: "FollowRequests");

            migrationBuilder.DropColumn(
                name: "FollowId",
                table: "FollowRequests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowRequests",
                table: "FollowRequests",
                columns: new[] { "SenderId", "FollowerId" });
        }
    }
}
