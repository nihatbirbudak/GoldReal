using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ıssoldatribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Properties");
        }
    }
}
