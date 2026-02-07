using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class isCoverUniq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropertyPhotos_PropertyId",
                table: "PropertyPhotos");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "PropertyPhotos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyPhotos_PropertyId",
                table: "PropertyPhotos",
                column: "PropertyId",
                unique: true,
                filter: "[IsCover] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropertyPhotos_PropertyId",
                table: "PropertyPhotos");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "PropertyPhotos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyPhotos_PropertyId",
                table: "PropertyPhotos",
                column: "PropertyId");
        }
    }
}
