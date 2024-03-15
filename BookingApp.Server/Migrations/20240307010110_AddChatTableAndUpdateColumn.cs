using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddChatTableAndUpdateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chat_User_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Chat_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ProductId",
                table: "Comment",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_ReceiverId",
                table: "Chat",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_SenderId",
                table: "Chat",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Product_ProductId",
                table: "Comment",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Product_ProductId",
                table: "Comment");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ProductId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Comment");
        }
    }
}
