using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalWorldOnline.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateCashShopDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_ItemInfo_ItemId",
                schema: "Asset",
                table: "ItemInfo",
                column: "ItemId");

            migrationBuilder.CreateTable(
                name: "CashShop",
                schema: "Shop",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UniqueId = table.Column<int>(type: "int", nullable: false),
                    IconId = table.Column<int>(type: "int", nullable: false),
                    SalesPercent = table.Column<int>(type: "int", nullable: false),
                    PurchaseCashType = table.Column<int>(type: "int", nullable: false),
                    SellingPrice = table.Column<int>(type: "int", nullable: false),
                    ItemsJson = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashShop", x => x.Id);
                    table.UniqueConstraint("AK_CashShop_UniqueId", x => x.UniqueId);
                });

            migrationBuilder.CreateTable(
                name: "CashShopItems",
                schema: "Shop",
                columns: table => new
                {
                    UniqueId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashShopItems", x => new { x.UniqueId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_CashShopItems_CashShop_UniqueId",
                        column: x => x.UniqueId,
                        principalSchema: "Shop",
                        principalTable: "CashShop",
                        principalColumn: "UniqueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CashShopItems_ItemInfo_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Asset",
                        principalTable: "ItemInfo",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashShopItems_ItemId",
                schema: "Shop",
                table: "CashShopItems",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashShopItems",
                schema: "Shop");

            migrationBuilder.DropTable(
                name: "CashShop",
                schema: "Shop");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ItemInfo_ItemId",
                schema: "Asset",
                table: "ItemInfo");
        }
    }
}
