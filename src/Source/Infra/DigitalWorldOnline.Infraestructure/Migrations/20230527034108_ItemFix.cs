using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalWorldOnline.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class ItemFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemAccessoryStatus_Item_CharacterItemId",
                schema: "Character",
                table: "ItemAccessoryStatus");

            migrationBuilder.DropTable(
                name: "ItemAccessoryStatus",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Item",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "ItemList",
                schema: "Account");

            migrationBuilder.EnsureSchema(
                name: "Shared");

            migrationBuilder.RenameTable(
                name: "ItemList",
                schema: "Character",
                newName: "ItemList",
                newSchema: "Shared");

            migrationBuilder.RenameTable(
                name: "ItemAccessoryStatus",
                schema: "Character",
                newName: "ItemAccessoryStatus",
                newSchema: "Shared");

            migrationBuilder.RenameTable(
                name: "Item",
                schema: "Character",
                newName: "Item",
                newSchema: "Shared");

            migrationBuilder.RenameColumn(
                name: "CharacterItemId",
                schema: "Shared",
                table: "ItemAccessoryStatus",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemAccessoryStatus_CharacterItemId",
                schema: "Shared",
                table: "ItemAccessoryStatus",
                newName: "IX_ItemAccessoryStatus_ItemId");

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

            migrationBuilder.AlterColumn<long>(
                name: "CharacterId",
                schema: "Shared",
                table: "ItemList",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                schema: "Shared",
                table: "ItemList",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Power",
                schema: "Shared",
                table: "Item",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)100,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_ItemList_AccountId",
                schema: "Shared",
                table: "ItemList",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemAccessoryStatus_Item_ItemId",
                schema: "Shared",
                table: "ItemAccessoryStatus",
                column: "ItemId",
                principalSchema: "Shared",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemList_Account_AccountId",
                schema: "Shared",
                table: "ItemList",
                column: "AccountId",
                principalSchema: "Account",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemAccessoryStatus_Item_ItemId",
                schema: "Shared",
                table: "ItemAccessoryStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemList_Account_AccountId",
                schema: "Shared",
                table: "ItemList");

            migrationBuilder.DropIndex(
                name: "IX_ItemList_AccountId",
                schema: "Shared",
                table: "ItemList");

            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "Shared",
                table: "ItemList");

            migrationBuilder.RenameTable(
                name: "ItemList",
                schema: "Shared",
                newName: "ItemList",
                newSchema: "Character");

            migrationBuilder.RenameTable(
                name: "ItemAccessoryStatus",
                schema: "Shared",
                newName: "ItemAccessoryStatus",
                newSchema: "Character");

            migrationBuilder.RenameTable(
                name: "Item",
                schema: "Shared",
                newName: "Item",
                newSchema: "Character");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                schema: "Character",
                table: "ItemAccessoryStatus",
                newName: "CharacterItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemAccessoryStatus_ItemId",
                schema: "Character",
                table: "ItemAccessoryStatus",
                newName: "IX_ItemAccessoryStatus_CharacterItemId");

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

            migrationBuilder.AlterColumn<long>(
                name: "CharacterId",
                schema: "Character",
                table: "ItemList",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Power",
                schema: "Character",
                table: "Item",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)100);

            migrationBuilder.CreateTable(
                name: "ItemList",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Bits = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Size = table.Column<byte>(type: "tinyint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemList_Account_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Account",
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemListId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ItemId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Power = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    RemainingTime = table.Column<int>(type: "int", nullable: false),
                    RerollLeft = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    TamerShopSellPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_ItemList_ItemListId",
                        column: x => x.ItemListId,
                        principalSchema: "Account",
                        principalTable: "ItemList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemAccessoryStatus",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<short>(type: "smallint", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAccessoryStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemAccessoryStatus_Item_AccountItemId",
                        column: x => x.AccountItemId,
                        principalSchema: "Account",
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemListId",
                schema: "Account",
                table: "Item",
                column: "ItemListId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAccessoryStatus_AccountItemId",
                schema: "Account",
                table: "ItemAccessoryStatus",
                column: "AccountItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemList_AccountId",
                schema: "Account",
                table: "ItemList",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemAccessoryStatus_Item_CharacterItemId",
                schema: "Character",
                table: "ItemAccessoryStatus",
                column: "CharacterItemId",
                principalSchema: "Character",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
