﻿using DigitalWorldOnline.Commons.DTOs.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalWorldOnline.Infraestructure.ContextConfiguration.Assets
{
    public class TamerSkillAssetConfiguration : IEntityTypeConfiguration<TamerSkillAssetDTO>
    {
        public void Configure(EntityTypeBuilder<TamerSkillAssetDTO> builder)
        {
            builder
                .ToTable("TamerSkill", "Asset")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.SkillId)
                .HasColumnType("int")
                .IsRequired();

            builder
                .Property(x => x.SkillCode)
                .HasColumnType("int")
                .IsRequired();

            builder
                .Property(x => x.Duration)
                .HasColumnType("int")
                .IsRequired();

      
        }
    }
}