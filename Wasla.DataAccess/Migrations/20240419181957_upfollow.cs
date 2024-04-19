using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class upfollow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowRequests",
                columns: table => new
                {
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FollowerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowRequests", x => new { x.SenderId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_FollowRequests_Customer_FollowerId",
                        column: x => x.FollowerId,
                        principalSchema: "Account",
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FollowRequests_Customer_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "Account",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowRequests_FollowerId",
                table: "FollowRequests",
                column: "FollowerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowRequests");
        }
    }
}
