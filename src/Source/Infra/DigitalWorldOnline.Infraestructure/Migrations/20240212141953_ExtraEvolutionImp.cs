using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalWorldOnline.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtraEvolutionImp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "WOSCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WISCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WASCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "THSCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "STSCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "NESCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LISCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LASCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ICSCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FISCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DASCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.CreateTable(
                name: "ExtraEvolutionNpc",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NpcId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraEvolutionNpc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtraEvolutionInformation",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndexId = table.Column<int>(type: "int", nullable: false),
                    ExtraNpcId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraEvolutionInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtraEvolutionInformation_ExtraEvolutionNpc_ExtraNpcId",
                        column: x => x.ExtraNpcId,
                        principalSchema: "Asset",
                        principalTable: "ExtraEvolutionNpc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtraEvolution",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DigimonId = table.Column<int>(type: "int", nullable: false),
                    RequiredLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    ExtraInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraEvolution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtraEvolution_ExtraEvolutionInformation_ExtraInfoId",
                        column: x => x.ExtraInfoId,
                        principalSchema: "Asset",
                        principalTable: "ExtraEvolutionInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtraEvolutionMaterial",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ExtraEvolutionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraEvolutionMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtraEvolutionMaterial_ExtraEvolution_ExtraEvolutionId",
                        column: x => x.ExtraEvolutionId,
                        principalSchema: "Asset",
                        principalTable: "ExtraEvolution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtraEvolutionRequired",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ExtraEvolutionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraEvolutionRequired", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtraEvolutionRequired_ExtraEvolution_ExtraEvolutionId",
                        column: x => x.ExtraEvolutionId,
                        principalSchema: "Asset",
                        principalTable: "ExtraEvolution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "Config",
                table: "Hash",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2024, 2, 12, 11, 19, 48, 868, DateTimeKind.Local).AddTicks(7987));

            migrationBuilder.UpdateData(
                schema: "Routine",
                table: "Routine",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "NextRunTime" },
                values: new object[] { new DateTime(2024, 2, 12, 11, 19, 48, 942, DateTimeKind.Local).AddTicks(1077), new DateTime(2024, 2, 13, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "IX_ExtraEvolution_ExtraInfoId",
                schema: "Asset",
                table: "ExtraEvolution",
                column: "ExtraInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraEvolutionInformation_ExtraNpcId",
                schema: "Asset",
                table: "ExtraEvolutionInformation",
                column: "ExtraNpcId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraEvolutionMaterial_ExtraEvolutionId",
                schema: "Asset",
                table: "ExtraEvolutionMaterial",
                column: "ExtraEvolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraEvolutionRequired_ExtraEvolutionId",
                schema: "Asset",
                table: "ExtraEvolutionRequired",
                column: "ExtraEvolutionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtraEvolutionMaterial",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "ExtraEvolutionRequired",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "ExtraEvolution",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "ExtraEvolutionInformation",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "ExtraEvolutionNpc",
                schema: "Asset");

            migrationBuilder.AlterColumn<decimal>(
                name: "WOSCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WISCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WASCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "THSCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "STSCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "NESCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LISCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LASCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ICSCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FISCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DASCD",
                schema: "Asset",
                table: "TitleStatus",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.UpdateData(
                schema: "Config",
                table: "Hash",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2024, 2, 9, 13, 40, 7, 549, DateTimeKind.Local).AddTicks(4096));

            migrationBuilder.UpdateData(
                schema: "Routine",
                table: "Routine",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "NextRunTime" },
                values: new object[] { new DateTime(2024, 2, 9, 13, 40, 7, 605, DateTimeKind.Local).AddTicks(662), new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
