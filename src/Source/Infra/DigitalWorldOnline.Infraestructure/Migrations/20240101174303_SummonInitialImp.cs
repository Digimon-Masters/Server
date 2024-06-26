using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalWorldOnline.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class SummonInitialImp : Migration
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

            migrationBuilder.AlterColumn<decimal>(
                name: "CastingTime",
                schema: "Asset",
                table: "SkillInfo",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Chance",
                schema: "Asset",
                table: "ScanRewardDetail",
                type: "numeric(38,17)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "NoticeTime",
                schema: "Asset",
                table: "MonsterSkillInfo",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AnimationDelay",
                schema: "Asset",
                table: "MonsterSkillInfo",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Z",
                schema: "Config",
                table: "MobLocation",
                type: "numeric(38,17)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Z",
                schema: "Digimon",
                table: "Location",
                type: "numeric(38,17)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Z",
                schema: "Character",
                table: "Location",
                type: "numeric(38,17)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Chance",
                schema: "Config",
                table: "ItemDropReward",
                type: "numeric(38,17)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "SuccessChance",
                schema: "Config",
                table: "Hatch",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BreakChance",
                schema: "Config",
                table: "Hatch",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Size",
                schema: "Config",
                table: "FruitSize",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Chance",
                schema: "Config",
                table: "FruitSize",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Chance",
                schema: "Asset",
                table: "ContainerReward",
                type: "numeric(38,17)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "SuccessChance",
                schema: "Config",
                table: "Clone",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BreakChance",
                schema: "Config",
                table: "Clone",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Chance",
                schema: "Config",
                table: "BitsDropReward",
                type: "numeric(38,17)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldDefaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Summon",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Maps = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summon", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SummonMobDTO",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<byte>(type: "tinyint", nullable: false),
                    ViewRange = table.Column<int>(type: "int", nullable: false),
                    HuntRange = table.Column<int>(type: "int", nullable: false),
                    ReactionType = table.Column<int>(type: "int", nullable: false),
                    Attribute = table.Column<int>(type: "int", nullable: false),
                    Element = table.Column<int>(type: "int", nullable: false),
                    Family1 = table.Column<int>(type: "int", nullable: false),
                    Family2 = table.Column<int>(type: "int", nullable: false),
                    Family3 = table.Column<int>(type: "int", nullable: false),
                    ASValue = table.Column<int>(type: "int", nullable: false),
                    ARValue = table.Column<int>(type: "int", nullable: false),
                    ATValue = table.Column<int>(type: "int", nullable: false),
                    BLValue = table.Column<int>(type: "int", nullable: false),
                    CTValue = table.Column<int>(type: "int", nullable: false),
                    DEValue = table.Column<int>(type: "int", nullable: false),
                    DSValue = table.Column<int>(type: "int", nullable: false),
                    EVValue = table.Column<int>(type: "int", nullable: false),
                    HPValue = table.Column<int>(type: "int", nullable: false),
                    HTValue = table.Column<int>(type: "int", nullable: false),
                    MSValue = table.Column<int>(type: "int", nullable: false),
                    WSValue = table.Column<int>(type: "int", nullable: false),
                    SummonDTOId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummonMobDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonMobDTO_Summon_SummonDTOId",
                        column: x => x.SummonDTOId,
                        principalSchema: "Config",
                        principalTable: "Summon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonMobDropReward",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinAmount = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    MaxAmount = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    MobId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummonMobDropReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonMobDropReward_SummonMobDTO_MobId",
                        column: x => x.MobId,
                        principalTable: "SummonMobDTO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonMobExpReward",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TamerExperience = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    DigimonExperience = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    NatureExperience = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    ElementExperience = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    SkillExperience = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    MobId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummonMobExpReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonMobExpReward_SummonMobDTO_MobId",
                        column: x => x.MobId,
                        principalTable: "SummonMobDTO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonMobLocation",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapId = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    X = table.Column<int>(type: "int", nullable: false, defaultValue: 5000),
                    Y = table.Column<int>(type: "int", nullable: false, defaultValue: 4500),
                    Z = table.Column<decimal>(type: "numeric(38,17)", nullable: false, defaultValue: 0m),
                    MobConfigId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummonMobLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonMobLocation_SummonMobDTO_MobConfigId",
                        column: x => x.MobConfigId,
                        principalTable: "SummonMobDTO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonBitsDropReward",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinAmount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MaxAmount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Chance = table.Column<decimal>(type: "numeric(38,17)", nullable: false, defaultValue: 0m),
                    DropRewardId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummonBitsDropReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonBitsDropReward_SummonMobDropReward_DropRewardId",
                        column: x => x.DropRewardId,
                        principalSchema: "Config",
                        principalTable: "SummonMobDropReward",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonMobItemDropReward",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MinAmount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MaxAmount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Chance = table.Column<decimal>(type: "numeric(38,17)", nullable: false, defaultValue: 0m),
                    DropRewardId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummonMobItemDropReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonMobItemDropReward_SummonMobDropReward_DropRewardId",
                        column: x => x.DropRewardId,
                        principalSchema: "Config",
                        principalTable: "SummonMobDropReward",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "Config",
                table: "Hash",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 1, 14, 43, 1, 835, DateTimeKind.Local).AddTicks(8436));

            migrationBuilder.UpdateData(
                schema: "Routine",
                table: "Routine",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "NextRunTime" },
                values: new object[] { new DateTime(2024, 1, 1, 14, 43, 1, 849, DateTimeKind.Local).AddTicks(8968), new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "IX_SummonBitsDropReward_DropRewardId",
                schema: "Config",
                table: "SummonBitsDropReward",
                column: "DropRewardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SummonMobDropReward_MobId",
                schema: "Config",
                table: "SummonMobDropReward",
                column: "MobId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SummonMobDTO_SummonDTOId",
                table: "SummonMobDTO",
                column: "SummonDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonMobExpReward_MobId",
                schema: "Config",
                table: "SummonMobExpReward",
                column: "MobId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SummonMobItemDropReward_DropRewardId",
                schema: "Config",
                table: "SummonMobItemDropReward",
                column: "DropRewardId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonMobLocation_MobConfigId",
                schema: "Config",
                table: "SummonMobLocation",
                column: "MobConfigId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SummonBitsDropReward",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "SummonMobExpReward",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "SummonMobItemDropReward",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "SummonMobLocation",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "SummonMobDropReward",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "SummonMobDTO");

            migrationBuilder.DropTable(
                name: "Summon",
                schema: "Config");

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

            migrationBuilder.AlterColumn<decimal>(
                name: "CastingTime",
                schema: "Asset",
                table: "SkillInfo",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Chance",
                schema: "Asset",
                table: "ScanRewardDetail",
                type: "numeric(18,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "NoticeTime",
                schema: "Asset",
                table: "MonsterSkillInfo",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AnimationDelay",
                schema: "Asset",
                table: "MonsterSkillInfo",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Z",
                schema: "Config",
                table: "MobLocation",
                type: "numeric(18,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Z",
                schema: "Digimon",
                table: "Location",
                type: "numeric(18,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Z",
                schema: "Character",
                table: "Location",
                type: "numeric(18,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Chance",
                schema: "Config",
                table: "ItemDropReward",
                type: "numeric(18,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "SuccessChance",
                schema: "Config",
                table: "Hatch",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BreakChance",
                schema: "Config",
                table: "Hatch",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Size",
                schema: "Config",
                table: "FruitSize",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Chance",
                schema: "Config",
                table: "FruitSize",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Chance",
                schema: "Asset",
                table: "ContainerReward",
                type: "numeric(18,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "SuccessChance",
                schema: "Config",
                table: "Clone",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BreakChance",
                schema: "Config",
                table: "Clone",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Chance",
                schema: "Config",
                table: "BitsDropReward",
                type: "numeric(18,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldDefaultValue: 0m);

            migrationBuilder.UpdateData(
                schema: "Config",
                table: "Hash",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2023, 12, 27, 3, 28, 0, 356, DateTimeKind.Local).AddTicks(6592));

            migrationBuilder.UpdateData(
                schema: "Routine",
                table: "Routine",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "NextRunTime" },
                values: new object[] { new DateTime(2023, 12, 27, 3, 28, 0, 381, DateTimeKind.Local).AddTicks(8090), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
