using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaraWeb.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Path = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CollectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Version = table.Column<string>(type: "TEXT", maxLength: 6, nullable: true),
                    Bpm = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Artist = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Audio = table.Column<string>(type: "TEXT", nullable: false),
                    Gap = table.Column<int>(type: "INTEGER", nullable: false),
                    Start = table.Column<int>(type: "INTEGER", nullable: false),
                    End = table.Column<int>(type: "INTEGER", nullable: false),
                    Cover = table.Column<string>(type: "TEXT", nullable: true),
                    Background = table.Column<string>(type: "TEXT", nullable: true),
                    Video = table.Column<string>(type: "TEXT", nullable: true),
                    VideoGap = table.Column<int>(type: "INTEGER", nullable: false),
                    Vocals = table.Column<string>(type: "TEXT", nullable: true),
                    Instrumental = table.Column<string>(type: "TEXT", nullable: true),
                    PreviewStart = table.Column<int>(type: "INTEGER", nullable: false),
                    MedleyStart = table.Column<int>(type: "INTEGER", nullable: false),
                    MedleyEnd = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Genres = table.Column<string>(type: "TEXT", nullable: true),
                    Language = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Editions = table.Column<string>(type: "TEXT", nullable: true),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    Creator = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ProvidedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    AudioUrl = table.Column<string>(type: "TEXT", nullable: true),
                    VideoUrl = table.Column<string>(type: "TEXT", nullable: true),
                    CoverUrl = table.Column<string>(type: "TEXT", nullable: true),
                    BackgroundUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Rendition = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    Errors = table.Column<string>(type: "TEXT", nullable: true),
                    Warnings = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Songs_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SongPlayers",
                columns: table => new
                {
                    SongId = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongPlayers", x => new { x.SongId, x.Number });
                    table.ForeignKey(
                        name: "FK_SongPlayers_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Songs_CollectionId",
                table: "Songs",
                column: "CollectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongPlayers");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Collections");
        }
    }
}
