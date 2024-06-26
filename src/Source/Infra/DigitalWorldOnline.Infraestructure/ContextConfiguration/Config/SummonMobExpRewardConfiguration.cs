﻿using DigitalWorldOnline.Commons.DTOs.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalWorldOnline.Infraestructure.ContextConfiguration.Config
{
    public class SummonMobExpRewardConfiguration : IEntityTypeConfiguration<SummonMobExpRewardDTO>
    {
        public void Configure(EntityTypeBuilder<SummonMobExpRewardDTO> builder)
        {
            builder
                .ToTable("SummonMobExpReward", "Config")
                .HasKey(x => x.Id);

            builder
                .Property(e => e.TamerExperience)
                .HasColumnType("bigint")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.DigimonExperience)
                .HasColumnType("bigint")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.NatureExperience)
                .HasColumnType("smallint")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.ElementExperience)
                .HasColumnType("smallint")
                .HasDefaultValue(0)
                .IsRequired();

            builder
                .Property(e => e.SkillExperience)
                .HasColumnType("smallint")
                .HasDefaultValue(0)
                .IsRequired();
        }
    }
}