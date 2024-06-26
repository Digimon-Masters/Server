﻿using DigitalWorldOnline.Commons.Enums.ClientEnums;
using DigitalWorldOnline.Commons.DTOs.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DigitalWorldOnline.Infraestructure.ContextConfiguration.Config
{
    public class MobConfigConfiguration : IEntityTypeConfiguration<MobConfigDTO>
    {
        public void Configure(EntityTypeBuilder<MobConfigDTO> builder)
        {
            builder
                .ToTable("Mob", "Config")
                .HasKey(x => x.Id);

            builder
                .Property(e => e.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(e => e.Level)
                .HasColumnType("tinyint")
                .HasDefaultValue(1)
                .IsRequired();

            builder
                .Property(e => e.Model)
                .HasColumnType("int")
                .IsRequired();

            builder
                .Property(e => e.Type)
                .HasColumnType("int")
                .IsRequired();

            builder
                .Property(e => e.RespawnInterval)
                .HasColumnType("int")
                .HasDefaultValue(5)
                .IsRequired();

            builder
                .Property(e => e.Class)
                .HasColumnType("int")
                .IsRequired();

            builder
                .Property(e => e.ARValue)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.ASValue)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.ATValue)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.BLValue)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.CTValue)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.DEValue)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.DSValue)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.EVValue)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.HPValue)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.HTValue)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.MSValue)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.WSValue)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.ViewRange)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.HuntRange)
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder
            .Property(e => e.Coliseum)
            .HasColumnType("bit")
            .HasDefaultValue(0)
            .IsRequired();

            builder
          .Property(e => e.Round)
          .HasColumnType("tinyint")
          .HasDefaultValue(0)
          .IsRequired();

            builder
                .Property(x => x.ColiseumMobType)
                .HasColumnType("int")
                .HasConversion(new ValueConverter<ColiseumMobTypeEnum, int>(
                    x => (int)x,
                    x => (ColiseumMobTypeEnum)x))
                .HasDefaultValue(ColiseumMobTypeEnum.Normal)
                .IsRequired();

            builder
            .Property(e => e.WeekDay)
            .HasColumnType("int")
            .HasConversion(new ValueConverter<DungeonDayOfWeekEnum, int>(
        x => (int)x,
                x => (DungeonDayOfWeekEnum)x))
            .IsRequired();

            builder
                .Property(x => x.ReactionType)
                .HasColumnType("int")
                .HasConversion(new ValueConverter<DigimonReactionTypeEnum, int>(
                    x => (int)x,
                    x => (DigimonReactionTypeEnum)x))
                .HasDefaultValue(DigimonReactionTypeEnum.Passive)
                .IsRequired();

            builder
                .Property(x => x.Attribute)
                .HasColumnType("int")
                .HasConversion(new ValueConverter<DigimonAttributeEnum, int>(
                    x => (int)x,
                    x => (DigimonAttributeEnum)x))
                .HasDefaultValue(DigimonAttributeEnum.None)
                .IsRequired();

            builder
                .Property(x => x.Element)
                .HasColumnType("int")
                .HasConversion(new ValueConverter<DigimonElementEnum, int>(
                    x => (int)x,
                    x => (DigimonElementEnum)x))
                .HasDefaultValue(DigimonElementEnum.Neutral)
                .IsRequired();

            builder
                .Property(x => x.Family1)
                .HasColumnType("int")
                .HasConversion(new ValueConverter<DigimonFamilyEnum, int>(
                    x => (int)x,
                    x => (DigimonFamilyEnum)x))
                .HasDefaultValue(DigimonFamilyEnum.None)
                .IsRequired();

            builder
                .Property(x => x.Family2)
                .HasColumnType("int")
                .HasConversion(new ValueConverter<DigimonFamilyEnum, int>(
                    x => (int)x,
                    x => (DigimonFamilyEnum)x))
                .HasDefaultValue(DigimonFamilyEnum.None)
                .IsRequired();

            builder
                .Property(x => x.Family3)
                .HasColumnType("int")
                .HasConversion(new ValueConverter<DigimonFamilyEnum, int>(
                    x => (int)x,
                    x => (DigimonFamilyEnum)x))
                .HasDefaultValue(DigimonFamilyEnum.None)
                .IsRequired();

            builder
                .HasOne(x => x.Location)
                .WithOne(x => x.MobConfig)
                .HasForeignKey<MobLocationConfigDTO>(x => x.MobConfigId);

            builder
                .HasOne(x => x.DropReward)
                .WithOne(x => x.Mob)
                .HasForeignKey<MobDropRewardConfigDTO>(x => x.MobId);

            builder
                .HasOne(x => x.ExpReward)
                .WithOne(x => x.Mob)
                .HasForeignKey<MobExpRewardConfigDTO>(x => x.MobId);
        }
    }
}