using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalWorldOnline.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class CharacterArenaDailyPointsImp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "DailyPoints",
               schema: "Character",
               columns: table => new
               {
                   Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                   InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                   Points = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                   CharacterId = table.Column<long>(type: "bigint", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_DailyPoints", x => x.Id);
                   table.ForeignKey(
                       name: "FK_DailyPoints_Tamer_CharacterId",
                       column: x => x.CharacterId,
                       principalSchema: "Character",
                       principalTable: "Tamer",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {           
        }
    }
}
