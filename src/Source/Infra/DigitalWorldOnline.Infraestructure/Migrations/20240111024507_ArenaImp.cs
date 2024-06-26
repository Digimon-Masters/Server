using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalWorldOnline.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class ArenaImp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema("Arena");

            migrationBuilder.CreateTable(
                name: "Ranking",
                schema: "Arena",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranking", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competitor",
                schema: "Arena",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    New = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    Points = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    Position = table.Column<byte>(type: "tinyint", nullable: false, defaultValueSql: "0"),
                    RankingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TamerId = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Competitor_Ranking_RankingId",
                        column: x => x.RankingId,
                        principalSchema: "Arena",
                        principalTable: "Ranking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Competitor_RankingId",
                schema: "Arena",
                table: "Competitor",
                column: "RankingId");
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Competitor",
                schema: "Arena");

            migrationBuilder.DropTable(
                name: "Ranking",
                schema: "Arena");
        }
    }
}
