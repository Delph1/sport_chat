using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laktaren.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClubHouseFieldsToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClubHouseOnly",
                table: "Posts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TargetTeamId",
                table: "Posts",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClubHouseOnly",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "TargetTeamId",
                table: "Posts");
        }
    }
}
