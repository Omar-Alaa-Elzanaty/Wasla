﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class dd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Lines_LineId",
                table: "Trips");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Lines_LineId",
                table: "Trips",
                column: "LineId",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
