using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaraW3B.Server.Songs.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddLastAnalyzeTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastAnalyzeTime",
                table: "Libraries",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastAnalyzeTime",
                table: "Libraries");
        }
    }
}
