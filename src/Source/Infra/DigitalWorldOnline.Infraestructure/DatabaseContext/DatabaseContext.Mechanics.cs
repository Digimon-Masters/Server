using DigitalWorldOnline.Infraestructure.ContextConfiguration.Mechanics;
using DigitalWorldOnline.Commons.DTOs.Mechanics;
using Microsoft.EntityFrameworkCore;

namespace DigitalWorldOnline.Infraestructure
{
    public partial class DatabaseContext : DbContext
    {
        public DbSet<GuildDTO> Guild { get; set; }
        public DbSet<GuildSkillDTO> GuildSkill { get; set; }
        public DbSet<GuildMemberDTO> GuildMember { get; set; }
        public DbSet<GuildAuthorityDTO> GuildAuthority { get; set; }
        public DbSet<GuildHistoricDTO> GuildHistoric { get; set; }

        internal static void MechanicsEntityConfiguration(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GuildConfiguration());
            builder.ApplyConfiguration(new GuildSkillConfiguration());
            builder.ApplyConfiguration(new GuildMemberConfiguration());
            builder.ApplyConfiguration(new GuildAuthorityConfiguration());
            builder.ApplyConfiguration(new GuildHistoricConfiguration());
        }
    }
}