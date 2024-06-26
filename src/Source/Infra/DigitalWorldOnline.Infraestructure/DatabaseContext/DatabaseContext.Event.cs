using DigitalWorldOnline.Infraestructure.ContextConfiguration.Event;
using DigitalWorldOnline.Commons.DTOs.Events;
using Microsoft.EntityFrameworkCore;

namespace DigitalWorldOnline.Infraestructure
{
    public partial class DatabaseContext : DbContext
    {
        public DbSet<TimeRewardDTO> TimeReward { get; set; }
        public DbSet<AttendanceRewardDTO> AttendanceReward { get; set; }

        internal static void EventEntityConfiguration(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TimeRewardConfiguration());
            builder.ApplyConfiguration(new AttendanceRewardConfiguration());
        }
    }
}