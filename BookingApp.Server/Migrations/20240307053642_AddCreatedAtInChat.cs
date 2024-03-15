using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtInChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Chat",
                type: "datetime2",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Chat");
        }
    }
}
