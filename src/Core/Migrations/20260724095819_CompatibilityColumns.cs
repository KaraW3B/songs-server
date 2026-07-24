using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaraW3B.Server.Songs.Core.Migrations
{
    /// <inheritdoc />
    public partial class CompatibilityColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VocalsConversion",
                table: "Songs",
                newName: "VocalsCompatibility");

            migrationBuilder.RenameColumn(
                name: "VideoConversion",
                table: "Songs",
                newName: "VideoCompatibility");

            migrationBuilder.RenameColumn(
                name: "InstrumentalConversion",
                table: "Songs",
                newName: "InstrumentalCompatibility");

            migrationBuilder.RenameColumn(
                name: "AudioConversion",
                table: "Songs",
                newName: "AudioCompatibility");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VocalsCompatibility",
                table: "Songs",
                newName: "VocalsConversion");

            migrationBuilder.RenameColumn(
                name: "VideoCompatibility",
                table: "Songs",
                newName: "VideoConversion");

            migrationBuilder.RenameColumn(
                name: "InstrumentalCompatibility",
                table: "Songs",
                newName: "InstrumentalConversion");

            migrationBuilder.RenameColumn(
                name: "AudioCompatibility",
                table: "Songs",
                newName: "AudioConversion");
        }
    }
}
