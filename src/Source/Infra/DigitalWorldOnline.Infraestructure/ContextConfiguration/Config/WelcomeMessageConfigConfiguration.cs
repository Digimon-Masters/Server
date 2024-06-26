using DigitalWorldOnline.Commons.DTOs.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalWorldOnline.Infraestructure.ContextConfiguration.Config
{
    public class WelcomeMessageConfigConfiguration : IEntityTypeConfiguration<WelcomeMessageConfigDTO>
    {
        public void Configure(EntityTypeBuilder<WelcomeMessageConfigDTO> builder)
        {
            builder
                .ToTable("WelcomeMessage", "Config")
                .HasKey(x => x.Id);

            builder
                .Property(e => e.Message)
                .HasColumnType("varchar")
                .HasMaxLength(150)
                .IsRequired();

            builder
                .Property(e => e.Enabled)
                .HasColumnType("bit")
                .IsRequired();

            builder
                .HasData(
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 1,
                        Message = "1 1 1 1 1 1 0 0 1 1 1",
                        Enabled = false
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 2,
                        Message = "Please, drink some water! :)",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 3,
                        Message = "Did you hear that?",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 4,
                        Message = "Remember to feed your pet.",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 5,
                        Message = "Not a Pokémon game.",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 6,
                        Message = "Warning: Chat may be toxic.",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 7,
                        Message = "Be yourself!",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 8,
                        Message = "Welcome to DSO!",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 9,
                        Message = "Do you like chocolate?",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 10,
                        Message = "Here we go again!",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 11,
                        Message = "Join our Discord! discord.gg/dsooficial",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 12,
                        Message = "Can you see that mountain over there?",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 13,
                        Message = "\"Look into the source\"",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 14,
                        Message = "The staff will NEVER ask your password!",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 15,
                        Message = "Y0ur br4in 1s am4z1ng!",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 16,
                        Message = "This is the rythm of the night! (8)",
                        Enabled = true
                    },
                    new WelcomeMessageConfigDTO()
                    {
                        Id = 17,
                        Message = "Happy new eyer !!!",
                        Enabled = false
                    }
                    );
        }
    }
}