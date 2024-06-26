using DigitalWorldOnline.Infraestructure.ContextConfiguration.Security;
using DigitalWorldOnline.Commons.DTOs.Account;
using DigitalWorldOnline.Commons.DTOs.Chat;
using Microsoft.EntityFrameworkCore;

namespace DigitalWorldOnline.Infraestructure
{
    public partial class DatabaseContext : DbContext
    {
        public DbSet<LoginTryDTO> LoginTry { get; set; }
        public DbSet<ChatMessageDTO> ChatMessage { get; set; }

        internal static void SecurityEntityConfiguration(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new LoginTryConfiguration());
            builder.ApplyConfiguration(new ChatMessageConfiguration());
        }
    }
}