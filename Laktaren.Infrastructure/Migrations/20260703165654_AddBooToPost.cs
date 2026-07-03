using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laktaren.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBooToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLike",
                table: "Reactions");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Reactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Reactions");

            migrationBuilder.AddColumn<bool>(
                name: "IsLike",
                table: "Reactions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
