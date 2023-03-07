using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Migrations
{
    /// <inheritdoc />
    public partial class addEntriesAndToUrlPair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "urlPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Entry",
                columns: table => new
                {
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlPairLongUrl = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entry", x => x.EntryDate);
                    table.ForeignKey(
                        name: "FK_Entry_urlPairs_UrlPairLongUrl",
                        column: x => x.UrlPairLongUrl,
                        principalTable: "urlPairs",
                        principalColumn: "LongUrl");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entry_UrlPairLongUrl",
                table: "Entry",
                column: "UrlPairLongUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entry");

            migrationBuilder.DropColumn(
                name: "User",
                table: "urlPairs");
        }
    }
}
