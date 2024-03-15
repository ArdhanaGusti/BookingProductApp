using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUniqueColumnForProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Product",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Slug",
                table: "Product",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_Slug",
                table: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
