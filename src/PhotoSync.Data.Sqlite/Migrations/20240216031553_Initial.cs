using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoSync.Data.Sqlite.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PhotoLibraries",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PhotoLibraries", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Destinations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                FullPath = table.Column<string>(type: "TEXT", nullable: false),
                LastSynced = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                PhotoLibraryId = table.Column<Guid>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Destinations", x => x.Id);
                table.ForeignKey(
                    name: "FK_Destinations_PhotoLibraries_PhotoLibraryId",
                    column: x => x.PhotoLibraryId,
                    principalTable: "PhotoLibraries",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "SourceFolders",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                FullPath = table.Column<string>(type: "TEXT", nullable: false),
                LastRefreshed = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                PhotoLibraryId = table.Column<Guid>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SourceFolders", x => x.Id);
                table.ForeignKey(
                    name: "FK_SourceFolders_PhotoLibraries_PhotoLibraryId",
                    column: x => x.PhotoLibraryId,
                    principalTable: "PhotoLibraries",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "ExcludedFolders",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                SourceFolderId = table.Column<Guid>(type: "TEXT", nullable: false),
                RelativePath = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ExcludedFolders", x => x.Id);
                table.ForeignKey(
                    name: "FK_ExcludedFolders_SourceFolders_SourceFolderId",
                    column: x => x.SourceFolderId,
                    principalTable: "SourceFolders",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Photos",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                SourceFolderId = table.Column<Guid>(type: "TEXT", nullable: false),
                RelativePath = table.Column<string>(type: "TEXT", nullable: false),
                ProcessAction = table.Column<int>(type: "INTEGER", nullable: false),
                SizeBytes = table.Column<long>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Photos", x => x.Id);
                table.ForeignKey(
                    name: "FK_Photos_SourceFolders_SourceFolderId",
                    column: x => x.SourceFolderId,
                    principalTable: "SourceFolders",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Destinations_FullPath",
            table: "Destinations",
            column: "FullPath",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Destinations_PhotoLibraryId",
            table: "Destinations",
            column: "PhotoLibraryId");

        migrationBuilder.CreateIndex(
            name: "IX_ExcludedFolders_RelativePath",
            table: "ExcludedFolders",
            column: "RelativePath",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ExcludedFolders_SourceFolderId",
            table: "ExcludedFolders",
            column: "SourceFolderId");

        migrationBuilder.CreateIndex(
            name: "IX_Photos_RelativePath",
            table: "Photos",
            column: "RelativePath",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Photos_SourceFolderId",
            table: "Photos",
            column: "SourceFolderId");

        migrationBuilder.CreateIndex(
            name: "IX_SourceFolders_FullPath",
            table: "SourceFolders",
            column: "FullPath",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_SourceFolders_PhotoLibraryId",
            table: "SourceFolders",
            column: "PhotoLibraryId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Destinations");

        migrationBuilder.DropTable(
            name: "ExcludedFolders");

        migrationBuilder.DropTable(
            name: "Photos");

        migrationBuilder.DropTable(
            name: "SourceFolders");

        migrationBuilder.DropTable(
            name: "PhotoLibraries");
    }
}
