using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace mmangold.com.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuruOrGreaterKanjis",
                columns: table => new
                {
                    Character = table.Column<string>(nullable: false),
                    UnlockedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuruOrGreaterKanjis", x => x.Character);
                });

            migrationBuilder.CreateTable(
                name: "GuruOrGreaterRadicals",
                columns: table => new
                {
                    ImageUri = table.Column<string>(nullable: false),
                    UnlockedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuruOrGreaterRadicals", x => x.ImageUri);
                });

            migrationBuilder.CreateTable(
                name: "GuruOrGreaterVocabs",
                columns: table => new
                {
                    Character = table.Column<string>(nullable: false),
                    UnlockedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuruOrGreaterVocabs", x => x.Character);
                });

            migrationBuilder.CreateTable(
                name: "LevelProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Level = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelProgresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleWeightLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(nullable: false),
                    DayOfYear = table.Column<int>(nullable: false),
                    Weight = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleWeightLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaniKaniSyncs",
                columns: table => new
                {
                    WaniKaniSyncId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SyncDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaniKaniSyncs", x => x.WaniKaniSyncId);
                });

            migrationBuilder.CreateTable(
                name: "WeightSyncs",
                columns: table => new
                {
                    WeightSyncId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NewRecords = table.Column<int>(nullable: false),
                    SyncDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightSyncs", x => x.WeightSyncId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuruOrGreaterKanjis_Character",
                table: "GuruOrGreaterKanjis",
                column: "Character",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuruOrGreaterRadicals_ImageUri",
                table: "GuruOrGreaterRadicals",
                column: "ImageUri",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuruOrGreaterVocabs_Character",
                table: "GuruOrGreaterVocabs",
                column: "Character",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuruOrGreaterKanjis");

            migrationBuilder.DropTable(
                name: "GuruOrGreaterRadicals");

            migrationBuilder.DropTable(
                name: "GuruOrGreaterVocabs");

            migrationBuilder.DropTable(
                name: "LevelProgresses");

            migrationBuilder.DropTable(
                name: "SimpleWeightLogs");

            migrationBuilder.DropTable(
                name: "WaniKaniSyncs");

            migrationBuilder.DropTable(
                name: "WeightSyncs");
        }
    }
}
