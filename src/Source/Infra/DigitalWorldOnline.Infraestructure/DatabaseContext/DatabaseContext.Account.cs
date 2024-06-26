using DigitalWorldOnline.Infraestructure.ContextConfiguration.Account;
using DigitalWorldOnline.Commons.DTOs.Account;
using Microsoft.EntityFrameworkCore;
using DigitalWorldOnline.Infraestructure.ContextConfiguration.Config;

namespace DigitalWorldOnline.Infraestructure
{
    public partial class DatabaseContext
    {
        public DbSet<AccountDTO> Account { get; set; }
        public DbSet<AccountBlockDTO> AccountBlock { get; set; }
        public DbSet<SystemInformationDTO> SystemInformation { get; set; }

        internal static void AccountEntityConfiguration(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AccountConfiguration());
            builder.ApplyConfiguration(new SystemInformationConfiguration());
            builder.ApplyConfiguration(new AccountBlockConfiguration());
        }
    }
}