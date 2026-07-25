using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaraW3B.Server.Songs.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddFFProbeInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioCompatibility",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "InstrumentalCompatibility",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "VideoCompatibility",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "VocalsCompatibility",
                table: "Songs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AudioCompatibility",
                table: "Songs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstrumentalCompatibility",
                table: "Songs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VideoCompatibility",
                table: "Songs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VocalsCompatibility",
                table: "Songs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
