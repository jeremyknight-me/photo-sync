using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoSync.Data.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhotoLibraries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    LastRefreshed = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LastSynced = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    SourceFolder = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoLibraries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExcludedFolders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RelativePath = table.Column<string>(type: "TEXT", nullable: false),
                    PhotoLibraryId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcludedFolders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcludedFolders_PhotoLibraries_PhotoLibraryId",
                        column: x => x.PhotoLibraryId,
                        principalTable: "PhotoLibraries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RelativePath = table.Column<string>(type: "TEXT", nullable: false),
                    ProcessAction = table.Column<int>(type: "INTEGER", nullable: false),
                    SizeBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    PhotoLibraryId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_PhotoLibraries_PhotoLibraryId",
                        column: x => x.PhotoLibraryId,
                        principalTable: "PhotoLibraries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExcludedFolders_PhotoLibraryId",
                table: "ExcludedFolders",
                column: "PhotoLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcludedFolders_RelativePath",
                table: "ExcludedFolders",
                column: "RelativePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PhotoLibraryId",
                table: "Photos",
                column: "PhotoLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_RelativePath",
                table: "Photos",
                column: "RelativePath",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcludedFolders");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "PhotoLibraries");
        }
    }
}
