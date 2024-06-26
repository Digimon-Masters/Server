using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalWorldOnline.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Account");

            migrationBuilder.EnsureSchema(
                name: "Character");

            migrationBuilder.EnsureSchema(
                name: "Event");

            migrationBuilder.EnsureSchema(
                name: "Digimon");

            migrationBuilder.EnsureSchema(
                name: "Guild");

            migrationBuilder.EnsureSchema(
                name: "Config");

            migrationBuilder.EnsureSchema(
                name: "Asset");

            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.EnsureSchema(
                name: "Shop");

            migrationBuilder.CreateTable(
                name: "Account",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: false),
                    SecondaryPassword = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    Email = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    AccessLevel = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    LastConnection = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MembershipExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Premium = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Silk = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastPlayedServer = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    LastPlayedCharacter = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    ReceiveWelcome = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buff",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuffId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DigimonSkillCode = table.Column<int>(type: "int", nullable: false),
                    SkillCode = table.Column<int>(type: "int", nullable: false),
                    MinLevel = table.Column<int>(type: "int", nullable: false),
                    ConditionLevel = table.Column<int>(type: "int", nullable: false),
                    Class = table.Column<short>(type: "smallint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    LifeType = table.Column<int>(type: "int", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buff", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterBaseStatus",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ASValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ARValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ATValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BLValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DEValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EVValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HPValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    WSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterBaseStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterLevelStatus",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 80001),
                    Level = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    ExpValue = table.Column<long>(type: "bigint", nullable: false),
                    ASValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ARValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ATValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BLValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DEValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EVValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HPValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    WSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterLevelStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Digiclone",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    EnchantLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    DigicloneEnchantStatus = table.Column<int>(type: "int", nullable: false),
                    MinimalValue = table.Column<short>(type: "smallint", nullable: false),
                    MaximumValue = table.Column<short>(type: "smallint", nullable: false),
                    UpgradeChance = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    BreakChance = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Digiclone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DigimonBaseInfo",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Level = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    ScaleType = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    ViewRange = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HuntRange = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ReactionType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Attribute = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Element = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Family1 = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Family2 = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Family3 = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ASValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ARValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ATValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BLValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DEValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EVValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HPValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    WSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigimonBaseInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DigimonLevelStatus",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    ExpValue = table.Column<long>(type: "bigint", nullable: false),
                    ASValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ARValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ATValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BLValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DEValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EVValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HPValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    WSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigimonLevelStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DigimonSkill",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Slot = table.Column<byte>(type: "tinyint", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigimonSkill", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EvolutionList",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolutionList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Guild",
                schema: "Guild",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    CurrentExperience = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Notice = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ExtraSlots = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guild", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ENName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    SkillCode = table.Column<long>(type: "bigint", nullable: false),
                    TamerMinLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    TamerMaxLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    DigimonMinLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    DigimonMaxLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    Target = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    OverLap = table.Column<short>(type: "smallint", nullable: false),
                    ItemPriceInDigicore = table.Column<int>(type: "int", nullable: false),
                    ItemSellPrice = table.Column<long>(type: "bigint", nullable: false),
                    ItemBoundType = table.Column<byte>(type: "tinyint", nullable: false),
                    UseTimesType = table.Column<byte>(type: "tinyint", nullable: false),
                    UsageTimeInMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemCraft",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SequencialId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    NpcId = table.Column<int>(type: "int", nullable: false),
                    SuccessRate = table.Column<byte>(type: "tinyint", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCraft", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoginTry",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Ip = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false, defaultValue: 4)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginTry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Map",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    MapRegionId = table.Column<int>(type: "int", nullable: false),
                    MapNumber = table.Column<int>(type: "int", nullable: false),
                    AditionalMapNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Map",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MapRegionList",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapRegionList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Progress",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgressId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Progress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SealDetail",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SealId = table.Column<int>(type: "int", nullable: false),
                    RequiredAmount = table.Column<short>(type: "smallint", nullable: false),
                    SequentialId = table.Column<short>(type: "smallint", nullable: false),
                    ASValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ARValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ATValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BLValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DEValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EVValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HPValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    WSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SealDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Server",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Maintenance = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    New = table.Column<bool>(type: "bit", nullable: false),
                    Overload = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Server", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillCode",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillCode = table.Column<long>(type: "bigint", nullable: false),
                    Comment = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillInfo",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    DSUsage = table.Column<int>(type: "int", nullable: false),
                    HPUsage = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    CastingTime = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Cooldown = table.Column<int>(type: "int", nullable: false),
                    MaxLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    RequiredPoints = table.Column<byte>(type: "tinyint", nullable: false),
                    Target = table.Column<byte>(type: "tinyint", nullable: false),
                    AreaOfEffect = table.Column<int>(type: "int", nullable: false),
                    AoEMinDamage = table.Column<int>(type: "int", nullable: false),
                    AoEMaxDamage = table.Column<int>(type: "int", nullable: false),
                    Range = table.Column<int>(type: "int", nullable: false),
                    UnlockLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    MemoryChips = table.Column<byte>(type: "tinyint", nullable: false),
                    FirstConditionCode = table.Column<int>(type: "int", nullable: false),
                    SecondConditionCode = table.Column<int>(type: "int", nullable: false),
                    ThirdConditionCode = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TitleStatus",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    SCD = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    LASCD = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    FISCD = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    ICSCD = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    LISCD = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    STSCD = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    NESCD = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    DASCD = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    THSCD = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    WASCD = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    WISCD = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    WOSCD = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    ASValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ARValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ATValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BLValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DEValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EVValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HPValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    WSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WelcomeMessage",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelcomeMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Xai",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    XGauge = table.Column<int>(type: "int", nullable: false),
                    XCrystals = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Xai", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountBlock",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Reason = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountBlock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountBlock_Account_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Account",
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemList",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<byte>(type: "tinyint", nullable: false),
                    Bits = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
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
                name: "SystemInformation",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cpu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Gpu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Ip = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemInformation_Account_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Account",
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evolution",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    EvolutionListId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evolution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evolution_EvolutionList_EvolutionListId",
                        column: x => x.EvolutionListId,
                        principalSchema: "Asset",
                        principalTable: "EvolutionList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Authority",
                schema: "Guild",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Class = table.Column<int>(type: "int", nullable: false, defaultValue: 4),
                    Title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Duty = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    GuildId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authority", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authority_Guild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "Guild",
                        principalTable: "Guild",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                schema: "Guild",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    GuildId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skill_Guild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "Guild",
                        principalTable: "Guild",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tamer",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Position = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Model = table.Column<int>(type: "int", nullable: false, defaultValue: 80001),
                    Level = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    Name = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ServerId = table.Column<long>(type: "bigint", nullable: false),
                    CurrentExperience = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Channel = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)255),
                    DigimonSlots = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)3),
                    CurrentHp = table.Column<int>(type: "int", nullable: false, defaultValue: 50),
                    CurrentDs = table.Column<int>(type: "int", nullable: false, defaultValue: 40),
                    XGauge = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    XCrystals = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    CurrentTitle = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    GuildId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tamer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tamer_Guild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "Guild",
                        principalTable: "Guild",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemCraftMaterial",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ItemCraftId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCraftMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemCraftMaterial_ItemCraft_ItemCraftId",
                        column: x => x.ItemCraftId,
                        principalSchema: "Asset",
                        principalTable: "ItemCraft",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mob",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Level = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    ScaleType = table.Column<byte>(type: "tinyint", nullable: false),
                    ViewRange = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HuntRange = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ReactionType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Attribute = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Element = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Family1 = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Family2 = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Family3 = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RespawnInterval = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    GameMapConfigId = table.Column<long>(type: "bigint", nullable: false),
                    ASValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ARValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ATValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BLValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DEValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EVValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HPValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    HTValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    WSValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mob", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mob_Map_GameMapConfigId",
                        column: x => x.GameMapConfigId,
                        principalSchema: "Config",
                        principalTable: "Map",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MapRegion",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Index = table.Column<byte>(type: "tinyint", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false, defaultValue: 6500),
                    Y = table.Column<int>(type: "int", nullable: false, defaultValue: 6500),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    MapRegionListId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapRegion_MapRegionList_MapRegionListId",
                        column: x => x.MapRegionListId,
                        principalSchema: "Asset",
                        principalTable: "MapRegionList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillCodeApply",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false, defaultValue: 101),
                    Attribute = table.Column<int>(type: "int", nullable: false, defaultValue: 20),
                    Value = table.Column<int>(type: "int", nullable: false),
                    AdditionalValue = table.Column<int>(type: "int", nullable: false),
                    SkillCodeAssetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillCodeApply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillCodeApply_SkillCode_SkillCodeAssetId",
                        column: x => x.SkillCodeAssetId,
                        principalSchema: "Asset",
                        principalTable: "SkillCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemListId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Amount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RemainingTime = table.Column<int>(type: "int", nullable: false),
                    TamerShopSellPrice = table.Column<int>(type: "int", nullable: false),
                    DigitaryPower = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    DigiablePowerRenewelNumber = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    Attributes1 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Attributes2 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Attributes3 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Attributes4 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat1 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat2 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat3 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat4 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat5 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat6 = table.Column<short>(type: "smallint", nullable: false),
                    Stat7 = table.Column<short>(type: "smallint", nullable: false),
                    Stat8 = table.Column<short>(type: "smallint", nullable: false),
                    Stat1Value = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat2Value = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat3Value = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat4Value = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat5Value = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat6Value = table.Column<short>(type: "smallint", nullable: false),
                    Stat7Value = table.Column<short>(type: "smallint", nullable: false),
                    Stat8Value = table.Column<short>(type: "smallint", nullable: false),
                    Unknown = table.Column<short>(type: "smallint", nullable: false),
                    Unknown1 = table.Column<short>(type: "smallint", nullable: false),
                    Unknown2 = table.Column<short>(type: "smallint", nullable: false),
                    Unknown3 = table.Column<short>(type: "smallint", nullable: false),
                    Unknown5 = table.Column<short>(type: "smallint", nullable: false),
                    Unknown22 = table.Column<short>(type: "smallint", nullable: false)
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
                name: "EvolutionLine",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvolutionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolutionLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvolutionLine_Evolution_EvolutionId",
                        column: x => x.EvolutionId,
                        principalSchema: "Asset",
                        principalTable: "Evolution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActiveEvolution",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DsPerSecond = table.Column<int>(type: "int", nullable: false),
                    XgPerSecond = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveEvolution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiveEvolution_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceReward",
                schema: "Event",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalDays = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceReward_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuffList",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuffList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuffList_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessage",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Message = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessage_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsignedShop",
                schema: "Shop",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Channel = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    ItemId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    GeneralHandler = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsignedShop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsignedShop_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Digimon",
                schema: "Digimon",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseType = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    Name = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Size = table.Column<short>(type: "smallint", nullable: false),
                    CurrentExperience = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    CurrentSkillExperience = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    TranscendenceExperience = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    HatchGrade = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    CurrentType = table.Column<int>(type: "int", nullable: false),
                    Friendship = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    CurrentHp = table.Column<int>(type: "int", nullable: false, defaultValue: 150),
                    CurrentDs = table.Column<int>(type: "int", nullable: false, defaultValue: 140),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Digimon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Digimon_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Foe",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Annotation = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    FoeId = table.Column<long>(type: "bigint", nullable: false),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Foe_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friend",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Annotation = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Connected = table.Column<bool>(type: "bit", nullable: false),
                    FriendId = table.Column<long>(type: "bigint", nullable: false),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friend_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incubator",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EggId = table.Column<int>(type: "int", nullable: false, defaultValue: -1),
                    HatchLevel = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    BackupDiskId = table.Column<int>(type: "int", nullable: false, defaultValue: -1),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incubator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incubator_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemList",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<byte>(type: "tinyint", nullable: false),
                    Bits = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemList_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false),
                    MapId = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    X = table.Column<int>(type: "int", nullable: false, defaultValue: 5000),
                    Y = table.Column<int>(type: "int", nullable: false, defaultValue: 4500),
                    Z = table.Column<decimal>(type: "numeric(38,17)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Location_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MapRegion",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unlocked = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapRegion_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                schema: "Guild",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contribution = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Authority = table.Column<int>(type: "int", nullable: false, defaultValue: 4),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false),
                    GuildId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Member_Guild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "Guild",
                        principalTable: "Guild",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Member_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Progress",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompletedData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompletedDataValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Progress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Progress_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SealList",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SealLeaderId = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SealList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SealList_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeReward",
                schema: "Event",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RewardIndex = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeReward_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XaiInfo",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    XGauge = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    XCrystals = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    CharacterId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XaiInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XaiInfo_Tamer_CharacterId",
                        column: x => x.CharacterId,
                        principalSchema: "Character",
                        principalTable: "Tamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MobDropReward",
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
                    table.PrimaryKey("PK_MobDropReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobDropReward_Mob_MobId",
                        column: x => x.MobId,
                        principalSchema: "Config",
                        principalTable: "Mob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MobExpReward",
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
                    table.PrimaryKey("PK_MobExpReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobExpReward_Mob_MobId",
                        column: x => x.MobId,
                        principalSchema: "Config",
                        principalTable: "Mob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MobLocation",
                schema: "Config",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MobConfigId = table.Column<long>(type: "bigint", nullable: false),
                    MapId = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    X = table.Column<int>(type: "int", nullable: false, defaultValue: 5000),
                    Y = table.Column<int>(type: "int", nullable: false, defaultValue: 4500),
                    Z = table.Column<decimal>(type: "numeric(38,17)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobLocation_Mob_MobConfigId",
                        column: x => x.MobConfigId,
                        principalSchema: "Config",
                        principalTable: "Mob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvolutionStage",
                schema: "Asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    EvolutionLineId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolutionStage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvolutionStage_EvolutionLine_EvolutionLineId",
                        column: x => x.EvolutionLineId,
                        principalSchema: "Asset",
                        principalTable: "EvolutionLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Buff",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuffListId = table.Column<long>(type: "bigint", nullable: false),
                    BuffId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Duration = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SkillId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buff_BuffList_BuffListId",
                        column: x => x.BuffListId,
                        principalSchema: "Character",
                        principalTable: "BuffList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConsignedShopLocationDTO",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsignedShopId = table.Column<long>(type: "bigint", nullable: false),
                    MapId = table.Column<short>(type: "smallint", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Z = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsignedShopLocationDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsignedShopLocationDTO_ConsignedShop_ConsignedShopId",
                        column: x => x.ConsignedShopId,
                        principalSchema: "Shop",
                        principalTable: "ConsignedShop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttributeExperience",
                schema: "Digimon",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<short>(type: "smallint", nullable: false),
                    Vaccine = table.Column<short>(type: "smallint", nullable: false),
                    Virus = table.Column<short>(type: "smallint", nullable: false),
                    Ice = table.Column<short>(type: "smallint", nullable: false),
                    Water = table.Column<short>(type: "smallint", nullable: false),
                    Fire = table.Column<short>(type: "smallint", nullable: false),
                    Land = table.Column<short>(type: "smallint", nullable: false),
                    Wind = table.Column<short>(type: "smallint", nullable: false),
                    Wood = table.Column<short>(type: "smallint", nullable: false),
                    Light = table.Column<short>(type: "smallint", nullable: false),
                    Dark = table.Column<short>(type: "smallint", nullable: false),
                    Thunder = table.Column<short>(type: "smallint", nullable: false),
                    Steel = table.Column<short>(type: "smallint", nullable: false),
                    DigimonId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeExperience_Digimon_DigimonId",
                        column: x => x.DigimonId,
                        principalSchema: "Digimon",
                        principalTable: "Digimon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuffList",
                schema: "Digimon",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DigimonId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuffList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuffList_Digimon_DigimonId",
                        column: x => x.DigimonId,
                        principalSchema: "Digimon",
                        principalTable: "Digimon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Digiclone",
                schema: "Digimon",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ATLevel = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    ATValue = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    BLLevel = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    BLValue = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    CTLevel = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    CTValue = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    EVLevel = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    EVValue = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    HPLevel = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    HPValue = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    DigimonId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Digiclone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Digiclone_Digimon_DigimonId",
                        column: x => x.DigimonId,
                        principalSchema: "Digimon",
                        principalTable: "Digimon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evolution",
                schema: "Digimon",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unlocked = table.Column<bool>(type: "bit", nullable: false),
                    SkillPoints = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    DigimonId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evolution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evolution_Digimon_DigimonId",
                        column: x => x.DigimonId,
                        principalSchema: "Digimon",
                        principalTable: "Digimon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                schema: "Digimon",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DigimonId = table.Column<long>(type: "bigint", nullable: false),
                    MapId = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    X = table.Column<int>(type: "int", nullable: false, defaultValue: 5000),
                    Y = table.Column<int>(type: "int", nullable: false, defaultValue: 4500),
                    Z = table.Column<decimal>(type: "numeric(38,17)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Location_Digimon_DigimonId",
                        column: x => x.DigimonId,
                        principalSchema: "Digimon",
                        principalTable: "Digimon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemListId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Amount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RemainingTime = table.Column<int>(type: "int", nullable: false),
                    TamerShopSellPrice = table.Column<int>(type: "int", nullable: false),
                    DigitaryPower = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    DigiablePowerRenewelNumber = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    Attributes1 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Attributes2 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Attributes3 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Attributes4 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat1 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat2 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat3 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat4 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat5 = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat6 = table.Column<short>(type: "smallint", nullable: false),
                    Stat7 = table.Column<short>(type: "smallint", nullable: false),
                    Stat8 = table.Column<short>(type: "smallint", nullable: false),
                    Stat1Value = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat2Value = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat3Value = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat4Value = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat5Value = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Stat6Value = table.Column<short>(type: "smallint", nullable: false),
                    Stat7Value = table.Column<short>(type: "smallint", nullable: false),
                    Stat8Value = table.Column<short>(type: "smallint", nullable: false),
                    Unknown = table.Column<short>(type: "smallint", nullable: false),
                    Unknown1 = table.Column<short>(type: "smallint", nullable: false),
                    Unknown2 = table.Column<short>(type: "smallint", nullable: false),
                    Unknown3 = table.Column<short>(type: "smallint", nullable: false),
                    Unknown5 = table.Column<short>(type: "smallint", nullable: false),
                    Unknown22 = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_ItemList_ItemListId",
                        column: x => x.ItemListId,
                        principalSchema: "Character",
                        principalTable: "ItemList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seal",
                schema: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SealId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Favorite = table.Column<bool>(type: "bit", nullable: false),
                    SequentialId = table.Column<short>(type: "smallint", nullable: false),
                    SealListId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seal_SealList_SealListId",
                        column: x => x.SealListId,
                        principalSchema: "Character",
                        principalTable: "SealList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BitsDropReward",
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
                    table.PrimaryKey("PK_BitsDropReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BitsDropReward_MobDropReward_DropRewardId",
                        column: x => x.DropRewardId,
                        principalSchema: "Config",
                        principalTable: "MobDropReward",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemDropReward",
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
                    table.PrimaryKey("PK_ItemDropReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDropReward_MobDropReward_DropRewardId",
                        column: x => x.DropRewardId,
                        principalSchema: "Config",
                        principalTable: "MobDropReward",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Buff",
                schema: "Digimon",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuffListId = table.Column<long>(type: "bigint", nullable: false),
                    BuffId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Duration = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SkillId = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buff_BuffList_BuffListId",
                        column: x => x.BuffListId,
                        principalSchema: "Digimon",
                        principalTable: "BuffList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvolutionSkill",
                schema: "Digimon",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentLevel = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    MaxLevel = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)20),
                    EvolutionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolutionSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvolutionSkill_Evolution_EvolutionId",
                        column: x => x.EvolutionId,
                        principalSchema: "Digimon",
                        principalTable: "Evolution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Config",
                table: "WelcomeMessage",
                columns: new[] { "Id", "Enabled", "Message" },
                values: new object[,]
                {
                    { 1L, false, "1 1 1 1 1 1 0 0 1 1 1" },
                    { 2L, true, "Please, drink some water! :)" },
                    { 3L, true, "Did you hear that?" },
                    { 4L, true, "Remember to feed your pet." },
                    { 5L, true, "Not a Pokémon game." },
                    { 6L, true, "Warning: Chat may be toxic." },
                    { 7L, true, "Be yourself!" },
                    { 8L, true, "Welcome to DSO!" },
                    { 9L, true, "Do you like chocolate?" },
                    { 10L, true, "Here we go again!" },
                    { 11L, true, "Join our Discord! discord.gg/dsooficial" },
                    { 12L, true, "Can you see that mountain over there?" },
                    { 13L, true, "\"Look into the source\"" },
                    { 14L, true, "The staff will NEVER ask your password!" },
                    { 15L, true, "Y0ur br4in 1s am4z1ng!" },
                    { 16L, true, "This is the rythm of the night! (8)" },
                    { 17L, false, "Happy new eyer !!!" }
                });

            migrationBuilder.InsertData(
                schema: "Asset",
                table: "Xai",
                columns: new[] { "Id", "ItemId", "XCrystals", "XGauge" },
                values: new object[] { 1L, 40017, (short)3, 2000 });

            migrationBuilder.CreateIndex(
                name: "IX_AccountBlock_AccountId",
                schema: "Account",
                table: "AccountBlock",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActiveEvolution_CharacterId",
                schema: "Character",
                table: "ActiveEvolution",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceReward_CharacterId",
                schema: "Event",
                table: "AttendanceReward",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttributeExperience_DigimonId",
                schema: "Digimon",
                table: "AttributeExperience",
                column: "DigimonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authority_GuildId",
                schema: "Guild",
                table: "Authority",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_BitsDropReward_DropRewardId",
                schema: "Config",
                table: "BitsDropReward",
                column: "DropRewardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buff_BuffListId",
                schema: "Character",
                table: "Buff",
                column: "BuffListId");

            migrationBuilder.CreateIndex(
                name: "IX_Buff_BuffListId",
                schema: "Digimon",
                table: "Buff",
                column: "BuffListId");

            migrationBuilder.CreateIndex(
                name: "IX_BuffList_CharacterId",
                schema: "Character",
                table: "BuffList",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuffList_DigimonId",
                schema: "Digimon",
                table: "BuffList",
                column: "DigimonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_CharacterId",
                schema: "Security",
                table: "ChatMessage",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignedShop_CharacterId",
                schema: "Shop",
                table: "ConsignedShop",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsignedShopLocationDTO_ConsignedShopId",
                table: "ConsignedShopLocationDTO",
                column: "ConsignedShopId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Digiclone_DigimonId",
                schema: "Digimon",
                table: "Digiclone",
                column: "DigimonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Digimon_CharacterId",
                schema: "Digimon",
                table: "Digimon",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Evolution_EvolutionListId",
                schema: "Asset",
                table: "Evolution",
                column: "EvolutionListId");

            migrationBuilder.CreateIndex(
                name: "IX_Evolution_DigimonId",
                schema: "Digimon",
                table: "Evolution",
                column: "DigimonId");

            migrationBuilder.CreateIndex(
                name: "IX_EvolutionLine_EvolutionId",
                schema: "Asset",
                table: "EvolutionLine",
                column: "EvolutionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvolutionSkill_EvolutionId",
                schema: "Digimon",
                table: "EvolutionSkill",
                column: "EvolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_EvolutionStage_EvolutionLineId",
                schema: "Asset",
                table: "EvolutionStage",
                column: "EvolutionLineId");

            migrationBuilder.CreateIndex(
                name: "IX_Foe_CharacterId",
                schema: "Character",
                table: "Foe",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_CharacterId",
                schema: "Character",
                table: "Friend",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Incubator_CharacterId",
                schema: "Character",
                table: "Incubator",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemListId",
                schema: "Account",
                table: "Item",
                column: "ItemListId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemListId",
                schema: "Character",
                table: "Item",
                column: "ItemListId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCraftMaterial_ItemCraftId",
                schema: "Asset",
                table: "ItemCraftMaterial",
                column: "ItemCraftId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDropReward_DropRewardId",
                schema: "Config",
                table: "ItemDropReward",
                column: "DropRewardId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemList_AccountId",
                schema: "Account",
                table: "ItemList",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemList_CharacterId",
                schema: "Character",
                table: "ItemList",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CharacterId",
                schema: "Character",
                table: "Location",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_DigimonId",
                schema: "Digimon",
                table: "Location",
                column: "DigimonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapRegion_MapRegionListId",
                schema: "Asset",
                table: "MapRegion",
                column: "MapRegionListId");

            migrationBuilder.CreateIndex(
                name: "IX_MapRegion_CharacterId",
                schema: "Character",
                table: "MapRegion",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_CharacterId",
                schema: "Guild",
                table: "Member",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_GuildId",
                schema: "Guild",
                table: "Member",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Mob_GameMapConfigId",
                schema: "Config",
                table: "Mob",
                column: "GameMapConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_MobDropReward_MobId",
                schema: "Config",
                table: "MobDropReward",
                column: "MobId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobExpReward_MobId",
                schema: "Config",
                table: "MobExpReward",
                column: "MobId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobLocation_MobConfigId",
                schema: "Config",
                table: "MobLocation",
                column: "MobConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Progress_CharacterId",
                schema: "Character",
                table: "Progress",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seal_SealListId",
                schema: "Character",
                table: "Seal",
                column: "SealListId");

            migrationBuilder.CreateIndex(
                name: "IX_SealList_CharacterId",
                schema: "Character",
                table: "SealList",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skill_GuildId",
                schema: "Guild",
                table: "Skill",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillCodeApply_SkillCodeAssetId",
                schema: "Asset",
                table: "SkillCodeApply",
                column: "SkillCodeAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemInformation_AccountId",
                schema: "Account",
                table: "SystemInformation",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tamer_GuildId",
                schema: "Character",
                table: "Tamer",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeReward_CharacterId",
                schema: "Event",
                table: "TimeReward",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_XaiInfo_CharacterId",
                schema: "Character",
                table: "XaiInfo",
                column: "CharacterId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountBlock",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "ActiveEvolution",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "AttendanceReward",
                schema: "Event");

            migrationBuilder.DropTable(
                name: "AttributeExperience",
                schema: "Digimon");

            migrationBuilder.DropTable(
                name: "Authority",
                schema: "Guild");

            migrationBuilder.DropTable(
                name: "BitsDropReward",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "Buff",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "Buff",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "Buff",
                schema: "Digimon");

            migrationBuilder.DropTable(
                name: "CharacterBaseStatus",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "CharacterLevelStatus",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "ChatMessage",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "ConsignedShopLocationDTO");

            migrationBuilder.DropTable(
                name: "Digiclone",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "Digiclone",
                schema: "Digimon");

            migrationBuilder.DropTable(
                name: "DigimonBaseInfo",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "DigimonLevelStatus",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "DigimonSkill",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "EvolutionSkill",
                schema: "Digimon");

            migrationBuilder.DropTable(
                name: "EvolutionStage",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "Foe",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "Friend",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "Incubator",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "Item",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Item",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "Item",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "ItemCraftMaterial",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "ItemDropReward",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "Location",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "Location",
                schema: "Digimon");

            migrationBuilder.DropTable(
                name: "LoginTry",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Map",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "MapRegion",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "MapRegion",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "Member",
                schema: "Guild");

            migrationBuilder.DropTable(
                name: "MobExpReward",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "MobLocation",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "Progress",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "Progress",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "Seal",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "SealDetail",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "Server",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Skill",
                schema: "Guild");

            migrationBuilder.DropTable(
                name: "SkillCodeApply",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "SkillInfo",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "SystemInformation",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "TimeReward",
                schema: "Event");

            migrationBuilder.DropTable(
                name: "TitleStatus",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "WelcomeMessage",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "Xai",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "XaiInfo",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "BuffList",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "BuffList",
                schema: "Digimon");

            migrationBuilder.DropTable(
                name: "ConsignedShop",
                schema: "Shop");

            migrationBuilder.DropTable(
                name: "Evolution",
                schema: "Digimon");

            migrationBuilder.DropTable(
                name: "EvolutionLine",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "ItemList",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "ItemList",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "ItemCraft",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "MobDropReward",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "MapRegionList",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "SealList",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "SkillCode",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "Digimon",
                schema: "Digimon");

            migrationBuilder.DropTable(
                name: "Evolution",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "Account",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Mob",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "Tamer",
                schema: "Character");

            migrationBuilder.DropTable(
                name: "EvolutionList",
                schema: "Asset");

            migrationBuilder.DropTable(
                name: "Map",
                schema: "Config");

            migrationBuilder.DropTable(
                name: "Guild",
                schema: "Guild");
        }
    }
}
