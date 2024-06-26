using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalWorldOnline.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class ItemDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attributes1",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Attributes2",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Attributes3",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Attributes4",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat1",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat1Value",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat2",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat2Value",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat3",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat3Value",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat4",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat4Value",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat5",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat5Value",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat6",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat6Value",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat7",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat7Value",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat8",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat8Value",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Unknown",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Unknown1",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Unknown2",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Unknown22",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Unknown3",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Unknown5",
                schema: "Character",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Attributes1",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Attributes2",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Attributes3",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Attributes4",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat1",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat1Value",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat2",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat2Value",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat3",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat3Value",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat4",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat4Value",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat5",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat5Value",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat6",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat6Value",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat7",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat7Value",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat8",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Stat8Value",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Unknown",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Unknown1",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Unknown2",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Unknown22",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Unknown3",
                schema: "Account",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Unknown5",
                schema: "Account",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "DigitaryPower",
                schema: "Character",
                table: "Item",
                newName: "RerollLeft");

            migrationBuilder.RenameColumn(
                name: "DigiablePowerRenewelNumber",
                schema: "Character",
                table: "Item",
                newName: "Power");

            migrationBuilder.RenameColumn(
                name: "DigitaryPower",
                schema: "Account",
                table: "Item",
                newName: "RerollLeft");

            migrationBuilder.RenameColumn(
                name: "DigiablePowerRenewelNumber",
                schema: "Account",
                table: "Item",
                newName: "Power");

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
                name: "UpgradeChance",
                schema: "Asset",
                table: "Digiclone",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BreakChance",
                schema: "Asset",
                table: "Digiclone",
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
                name: "ItemAccessoryStatus",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<short>(type: "smallint", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAccessoryStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemAccessoryStatus_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Account",
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemAccessoryStatus_Item_ItemId1",
                        column: x => x.ItemId,
                        principalSchema: "Character",
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemAccessoryStatus_ItemId",
                schema: "Character",
                table: "ItemAccessoryStatus",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemAccessoryStatus",
                schema: "Character");

            migrationBuilder.RenameColumn(
                name: "RerollLeft",
                schema: "Character",
                table: "Item",
                newName: "DigitaryPower");

            migrationBuilder.RenameColumn(
                name: "Power",
                schema: "Character",
                table: "Item",
                newName: "DigiablePowerRenewelNumber");

            migrationBuilder.RenameColumn(
                name: "RerollLeft",
                schema: "Account",
                table: "Item",
                newName: "DigitaryPower");

            migrationBuilder.RenameColumn(
                name: "Power",
                schema: "Account",
                table: "Item",
                newName: "DigiablePowerRenewelNumber");

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

            migrationBuilder.AddColumn<short>(
                name: "Attributes1",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Attributes2",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Attributes3",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Attributes4",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat1",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat1Value",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat2",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat2Value",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat3",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat3Value",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat4",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat4Value",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat5",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat5Value",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat6",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat6Value",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat7",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat7Value",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat8",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat8Value",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Unknown",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Unknown1",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Unknown2",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Unknown22",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Unknown3",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Unknown5",
                schema: "Character",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Attributes1",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Attributes2",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Attributes3",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Attributes4",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat1",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat1Value",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat2",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat2Value",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat3",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat3Value",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat4",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat4Value",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat5",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat5Value",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat6",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat6Value",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat7",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat7Value",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat8",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Stat8Value",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Unknown",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Unknown1",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Unknown2",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Unknown22",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Unknown3",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Unknown5",
                schema: "Account",
                table: "Item",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AlterColumn<decimal>(
                name: "UpgradeChance",
                schema: "Asset",
                table: "Digiclone",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BreakChance",
                schema: "Asset",
                table: "Digiclone",
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
        }
    }
}
