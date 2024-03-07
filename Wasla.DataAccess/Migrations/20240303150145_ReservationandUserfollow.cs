using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ReservationandUserfollow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_TripTimeTables_TripTimeTableId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Trips_TripId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollow_Customer_CustomerId",
                table: "UserFollow");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollow_Customer_FollowerId",
                table: "UserFollow");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_TripId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFollow",
                table: "UserFollow");

            migrationBuilder.DropIndex(
                name: "IX_UserFollow_CustomerId",
                table: "UserFollow");

            migrationBuilder.DropIndex(
                name: "IX_UserFollow_FollowerId",
                table: "UserFollow");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Reservations");

            migrationBuilder.RenameTable(
                name: "UserFollow",
                newName: "UserFollows");

            migrationBuilder.RenameColumn(
                name: "TripTimeTableId",
                table: "Reservations",
                newName: "TriptimeTableId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_TripTimeTableId",
                table: "Reservations",
                newName: "IX_Reservations_TriptimeTableId");

            migrationBuilder.AlterColumn<int>(
                name: "TriptimeTableId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFollows",
                table: "UserFollows",
                columns: new[] { "CustomerId", "FollowerId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowerId",
                table: "UserFollows",
                column: "FollowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_TripTimeTables_TriptimeTableId",
                table: "Reservations",
                column: "TriptimeTableId",
                principalTable: "TripTimeTables",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_Customer_CustomerId",
                table: "UserFollows",
                column: "CustomerId",
                principalSchema: "Account",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_Customer_FollowerId",
                table: "UserFollows",
                column: "FollowerId",
                principalSchema: "Account",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_TripTimeTables_TriptimeTableId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_Customer_CustomerId",
                table: "UserFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_Customer_FollowerId",
                table: "UserFollows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFollows",
                table: "UserFollows");

            migrationBuilder.DropIndex(
                name: "IX_UserFollows_FollowerId",
                table: "UserFollows");

            migrationBuilder.RenameTable(
                name: "UserFollows",
                newName: "UserFollow");

            migrationBuilder.RenameColumn(
                name: "TriptimeTableId",
                table: "Reservations",
                newName: "TripTimeTableId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_TriptimeTableId",
                table: "Reservations",
                newName: "IX_Reservations_TripTimeTableId");

            migrationBuilder.AlterColumn<int>(
                name: "TripTimeTableId",
                table: "Reservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFollow",
                table: "UserFollow",
                columns: new[] { "CustomerId", "FollowerId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TripId",
                table: "Reservations",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollow_CustomerId",
                table: "UserFollow",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFollow_FollowerId",
                table: "UserFollow",
                column: "FollowerId",
                unique: true);

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
                name: "FK_UserFollow_Customer_CustomerId",
                table: "UserFollow",
                column: "CustomerId",
                principalSchema: "Account",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollow_Customer_FollowerId",
                table: "UserFollow",
                column: "FollowerId",
                principalSchema: "Account",
                principalTable: "Customer",
                principalColumn: "Id");
        }
    }
}
