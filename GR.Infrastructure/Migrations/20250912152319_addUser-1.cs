using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addUser1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "AspNetUsers");
        }
    }
}
